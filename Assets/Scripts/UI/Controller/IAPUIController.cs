using Services;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class IAPUIController : MonoBehaviour
    {
        [SerializeField] private GameObject paidPanelPage;
        [SerializeField] private GameObject purchasePanelPage;
        
        [Inject] private readonly IAPService _iapService;

        private void Start()
        {
            _iapService.OnInitializeCompleted.AddListener(OnInitializeCompleted);
            _iapService.OnPurchaseCompleted.AddListener(OnPurchaseCompleted);
            _iapService.Initialize();
        }

        public void CloseButton()
        {
            purchasePanelPage.SetActive(false);
            paidPanelPage.SetActive(false);
        }

        public void PurchaseButton()
        {
            _iapService.Purchase(ProductType.Weekly);
        }

        private void OnInitializeCompleted()
        {
            Debug.Log("on initialize completed");
            //todo: make ui (prices)
        }
        
        private void OnPurchaseCompleted()
        {
            Debug.Log("on purchase completed");
            paidPanelPage.SetActive(true);
            purchasePanelPage.SetActive(false);
        }
    }
}