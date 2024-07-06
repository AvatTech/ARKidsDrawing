using Services;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class IAPUIController : MonoBehaviour
    {
        [SerializeField] private GameObject paidPanelPage;
        [SerializeField] private GameObject purchasePanelPage;
        [SerializeField] private UnityEngine.UI.Button purchaseButton;
        [SerializeField] private UnityEngine.UI.Button privacyPolicyButton;
        [SerializeField] private UnityEngine.UI.Button termsConditionButton;
        [SerializeField] private UnityEngine.UI.Button retireButton;
        
        [Inject] private readonly IAPService _iapService;

        private void Start()
        {
            Init();
            _iapService.OnInitializeCompleted.AddListener(OnInitializeCompleted);
            _iapService.OnPurchaseCompleted.AddListener(OnPurchaseCompleted);
            _iapService.Initialize();
        }

        private void Init()
        {
            InitComponents();
            InitCommands();
        }

        private void InitComponents()
        {
            if (purchaseButton != null)
            {
                purchaseButton.enabled = false;
            }
        }

        private void InitCommands()
        {
            privacyPolicyButton.onClick.AddListener(OnPrivacyClicked);
            termsConditionButton.onClick.AddListener(OnTermsClicked);
        }

        public void CloseButton()
        {
            purchasePanelPage.SetActive(false);
            paidPanelPage.SetActive(false);
        }

        public void PurchaseButton()
        {
            _iapService.Purchase(ProductType.SubscriptionWeekly);
        }

        private void OnInitializeCompleted()
        {
            Debug.Log("on initialize completed");
            //todo: make ui (prices)
            if (purchaseButton != null)
            {
                purchaseButton.enabled = true;
            }
        }
        
        private void OnPurchaseCompleted()
        {
            Debug.Log("on purchase completed");
            paidPanelPage.SetActive(true);
            purchasePanelPage.SetActive(false);
        }
        
        private void OnPrivacyClicked()
        {
            Application.OpenURL("https://doc-hosting.flycricket.io/arkidsdrawingapp/c7e33f88-dce1-40e4-9bb4-14ae65627321/privacy");
        }
    
        private void OnTermsClicked()
        {
            Application.OpenURL("https://doc-hosting.flycricket.io/terms/fcd9baaf-fa4d-41d7-b1b4-2b7e0795e318/terms");
        }
    }
}