using Repositories;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using Zenject;

namespace Services
{
    public class IAPService : IStoreListener
    {
        [Inject] private readonly IAPRepository _iapRepository;
        
        private const string AssetPath = "IAPProductCatalog";
        private const string WeeklyID = "premium_feature";
        private const string MonthlyID = "premium_feature";

        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;

        public UnityEvent OnInitializeCompleted { get; } = new();
        public UnityEvent OnPurchaseCompleted { get; } = new();

        public void Initialize()
        {
            InitializePurchase();
        }

        public void Purchase(ProductType type)
        {
            if (type == ProductType.Weekly)
            {
                _storeController.InitiatePurchase(_storeController.products.WithID(WeeklyID));
            }
            else if (type == ProductType.Monthly)
            {
                _storeController.InitiatePurchase(_storeController.products.WithID(MonthlyID));
            }
        }

        private async void InitializePurchase()
        {
            var option = new InitializationOptions();
            await UnityServices.InitializeAsync(option);
            var operation = Resources.LoadAsync<TextAsset>(AssetPath);
            operation.completed += HandleIAPCatalogLoaded;
        }

        private void HandleIAPCatalogLoaded(AsyncOperation operation)
        {
            var request = operation as ResourceRequest;

            if (request == null) return;

            if (request.asset as TextAsset == null) return;

            Debug.Log($"Loaded Asset: {request.asset}");

            var catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);

            Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

            StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#if UNITY_ANDROID
            var builder = ConfigurationBuilder.Instance(
                StandardPurchasingModule.Instance(AppStore.GooglePlay)
            );
#elif UNITY_IOS
        var builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore)
        );
#else
        var builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified)
        );
#endif
            foreach (var product in catalog.allProducts)
            {
                builder.AddProduct(product.id, product.type);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _extensionProvider = extensions;
            OnInitializeCompleted.Invoke();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("IAP Initialization Failed: " + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log("IAP Initialization Failed: " + message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Purchase Successful {purchaseEvent.purchasedProduct.definition.id}");
            UnlockPremiumFeatures();
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase Failed: product id:{product.definition.id} because {failureReason}");
        }

        private void UnlockPremiumFeatures()
        {
            Debug.Log("Premium Features Purchased.");
            _iapRepository.SetPurchase(true);
            OnPurchaseCompleted.Invoke();
        }

        private void CheckForPremium()
        {
            
        }
    }
}