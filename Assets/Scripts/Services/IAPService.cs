using System;
using Repositories;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using Utills;
using Zenject;

namespace Services
{
    public class IAPService : IStoreListener
    {
        [Inject] private readonly IAPRepository _iapRepository;

        private const string AssetPath = "IAPProductCatalog";

        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;

        public UnityEvent<string, string, string, string> OnInitializeCompleted { get; } = new();
        public UnityEvent OnPurchaseCompleted { get; } = new();

        public void Initialize()
        {
            InitializePurchase();
        }

        public void Purchase(ProductType type)
        {
            if (type == ProductType.SubscriptionWeekly)
            {
                _storeController.InitiatePurchase(_storeController.products.WithID(Constants.WeeklySubscriptionID));
            }
            else if (type == ProductType.SubscriptionMonthly)
            {
                _storeController.InitiatePurchase(_storeController.products.WithID(Constants.MonthlySubscriptionID));
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
            var weeklyProduct = _storeController.products.WithID(Constants.WeeklySubscriptionID);
            var monthlyProduct = _storeController.products.WithID(Constants.MonthlySubscriptionID);
            Debug.Log(
                $"{weeklyProduct.metadata.localizedTitle} {weeklyProduct.metadata.localizedPriceString} {monthlyProduct.metadata.localizedTitle} {monthlyProduct.metadata.localizedPriceString}");
            OnInitializeCompleted.Invoke(
                weeklyProduct.metadata.localizedTitle,
                weeklyProduct.metadata.localizedPriceString,
                monthlyProduct.metadata.localizedTitle,
                monthlyProduct.metadata.localizedPriceString
            );
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
            UnlockPremiumFeatures(purchaseEvent.purchasedProduct);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase Failed: product id:{product.definition.id} because {failureReason}");
        }

        private void UnlockPremiumFeatures(Product product)
        {
            Debug.Log("Premium Features Purchased.");
            // create a SubscriptionManager object using the subscription Product object
            var result = CheckSubscriptionStatus(product);

            if (result.HasValue)
            {
                _iapRepository.SaveSubscriptionExpiryDate(result.Value);
                OnPurchaseCompleted.Invoke();
            }
        }

        private DateTime? CheckSubscriptionStatus(Product product)
        {
            var subscriptionManager = new SubscriptionManager(product, null);
            var subscriptionInfo = subscriptionManager.getSubscriptionInfo();
            // check if the subscription is active
            if (subscriptionInfo.isSubscribed() == Result.True)
            {
                Debug.Log("Subscription expiration date: " + subscriptionInfo.getExpireDate());
                return subscriptionInfo.getExpireDate();
            }

            return null;
        }

        public bool Restore()
        {
            var product = _storeController.products.WithID(Constants.WeeklySubscriptionID);
            Debug.Log(product);
            if (product != null && product.hasReceipt)
            {
                var result = CheckSubscriptionStatus(product);
                if (result.HasValue)
                {
                    _iapRepository.SaveSubscriptionExpiryDate(result.Value);
                    OnPurchaseCompleted.Invoke();
                    return true;
                }

                return false;
            }

            product = _storeController.products.WithID(Constants.MonthlySubscriptionID);
            Debug.Log(product);
            if (product != null && product.hasReceipt)
            {
                var result = CheckSubscriptionStatus(product);
                if (result.HasValue)
                {
                    _iapRepository.SaveSubscriptionExpiryDate(result.Value);
                    OnPurchaseCompleted.Invoke();
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}