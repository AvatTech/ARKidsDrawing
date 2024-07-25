using System;
using Repositories;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Utills;
using Zenject;

namespace Services
{
    public class IAPService : IDetailedStoreListener
    {
        [Inject] private IAPRepository _iapRepository;

        private IStoreController _storeController;

        public UnityEvent<Product[]> OnInitializeCompleted { get; } = new();
        public UnityEvent OnPurchaseCompleted { get; } = new();
        public UnityEvent OnRestoreCompleted { get; } = new();

        public IAPService()
        {
            OnInitializeCompleted.AddListener(InitializeCompleted);
            Initialize();
        }

        public void Initialize()
        {
            if (_storeController == null)
            {
                InitializePurchasing();
                return;
            }

            OnInitializeCompleted.Invoke(new[]
            {
                _storeController.products.WithID(Constants.WeeklySubscriptionID),
                _storeController.products.WithID(Constants.MonthlySubscriptionID)
            });
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("IAPService: In-App Purchasing successfully initialized");
            _storeController = controller;

            OnInitializeCompleted.Invoke(new[]
            {
                controller.products.WithID(Constants.WeeklySubscriptionID),
                controller.products.WithID(Constants.MonthlySubscriptionID)
            });
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"IAPService: Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // Retrieve the purchased product
            var product = args.purchasedProduct;

            Debug.Log($"IAPService: Purchase Complete - Product: {product.definition.id}");

            if (args.purchasedProduct.definition.id == Constants.WeeklySubscriptionID)
            {
                _iapRepository.SaveSubscriptionExpiryDate(DateTime.Now.AddDays(7));
            }
            else if (args.purchasedProduct.definition.id == Constants.MonthlySubscriptionID)
            {
                _iapRepository.SaveSubscriptionExpiryDate(DateTime.Now.AddDays(30));
            }

            OnPurchaseCompleted.Invoke();

            // We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(
                $"IAPService: Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                      $" Purchase failure reason: {failureDescription.reason}," +
                      $" Purchase failure details: {failureDescription.message}");
        }


        private void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add our purchasable product and indicate its type.

            Debug.Log(
                $"IAPService: weekly id:{Constants.WeeklySubscriptionID}, monthly id:{Constants.MonthlySubscriptionID}");

            builder.AddProduct(Constants.WeeklySubscriptionID, ProductType.Subscription);
            builder.AddProduct(Constants.MonthlySubscriptionID, ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
        }

        private DateTime? IsSubscribedTo(Product subscription)
        {
            // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
            if (subscription.receipt == null)
            {
                return null;
            }

            //The intro_json parameter is optional and is only used for the App Store to get introductory information.
            var subscriptionManager = new SubscriptionManager(subscription, null);

            // The SubscriptionInfo contains all of the information about the subscription.
            // Find out more: https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPSubscriptionProducts.html
            var info = subscriptionManager.getSubscriptionInfo();

            if (info.isSubscribed() == Result.True)
            {
                return info.getExpireDate();
            }

            return null;
        }

        public void Purchase(string id)
        {
            _storeController.InitiatePurchase(id);
        }

        public void Restore()
        {
            var weeklyProduct = _storeController.products.WithID(Constants.WeeklySubscriptionID);
            var result = IsSubscribedTo(weeklyProduct);
            if (result.HasValue)
            {
                Debug.Log($"IAPService: Restore - Subscribe to: {weeklyProduct.definition.id}");
                _iapRepository.SaveSubscriptionExpiryDate(result.Value);
                OnRestoreCompleted.Invoke();
                return;
            }

            var monthlyProduct = _storeController.products.WithID(Constants.MonthlySubscriptionID);
            result = IsSubscribedTo(monthlyProduct);
            if (result.HasValue)
            {
                Debug.Log($"IAPService: Restore - Subscribe to: {monthlyProduct.definition.id}");
                _iapRepository.SaveSubscriptionExpiryDate(result.Value);
                OnRestoreCompleted.Invoke();
                return;
            }

            OnRestoreCompleted.Invoke();
            Debug.Log($"IAPService: Restore - Not Subscribed");
        }

        public bool IsPurchased()
        {
            return _iapRepository.IsPurchased();
        }

        private void InitializeCompleted(Product[] products)
        {
            OnInitializeCompleted.RemoveListener(InitializeCompleted);
            Restore();
        }
    }
}