using System;
using Bayegan.Builder;
using Bayegan.Storage.Abstractions;
using UnityEngine;
using UnityEngine.Purchasing;
using Utills;

namespace Repositories
{
    public class IAPRepository
    {
        private const string IAPKey = "IAPKey";
        private readonly IBayeganDictionary _bayegan = new BayeganDictionaryBuilder().Build();

        // public void SetPurchase(bool isPurchased)
        // {
        //     _bayegan.Store(IAPKey, isPurchased);
        // }

        public bool IsPurchased()
        {
            var expiryDateStr = _bayegan.Load<string>(IAPKey, "");
            Debug.Log(expiryDateStr);
            if (string.IsNullOrEmpty(expiryDateStr))
            {
                return false;
            }

            var expiryDate = DateTime.Parse(expiryDateStr);
            return DateTime.Now <= expiryDate;
        }

        public void SaveSubscriptionExpiryDate(Product product)
        {
            var purchaseDate = DateTime.Now;
            DateTime expiryDate;

            if (product.definition.id == Constants.WeeklySubscriptionID)
            {
                expiryDate = purchaseDate.AddDays(7);
            }
            else if (product.definition.id == Constants.MonthlySubscriptionID)
            {
                expiryDate = purchaseDate.AddMonths(1);
            }
            else
            {
                return;
            }

            _bayegan.Store(IAPKey, expiryDate.ToString());
        }
    }
}