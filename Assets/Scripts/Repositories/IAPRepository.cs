using Bayegan.Builder;
using Bayegan.Storage.Abstractions;

namespace Repositories
{
    public class IAPRepository
    {
        private const string IAPKey = "IAPKey";
        private readonly IBayeganDictionary _bayegan = new BayeganDictionaryBuilder().Build();

        public void SetPurchase(bool isPurchased)
        {
            _bayegan.Store(IAPKey, isPurchased);
        }

        public bool IsPurchased(bool defaultValue)
        {
            
            return _bayegan.Load(IAPKey, defaultValue);
        }
    }
}