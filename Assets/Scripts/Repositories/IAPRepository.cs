using System;
using Bayegan.Builder;
using Bayegan.Storage.Abstractions;
using UnityEngine.Purchasing;
using Utills;

namespace Repositories
{
    public class IAPRepository
    {
        private const string IAPKey = "IAPKey";
        private readonly IBayeganDictionary _bayegan = new BayeganDictionaryBuilder().Build();

        public bool IsPurchased()
        {
            var expiryDateStr = _bayegan.Load(IAPKey, "");
            if (string.IsNullOrEmpty(expiryDateStr))
            {
                return false;
            }

            var expiryDate = DateTime.Parse(expiryDateStr);
            return DateTime.Now <= expiryDate;
        }

        public void SaveSubscriptionExpiryDate(DateTime expireDate)
        {
            _bayegan.Store(IAPKey, expireDate.ToString());
        }
    }
}