using EasyUI.Toast;
using Services;
using UI.Toggle;
using UnityEngine;
using UnityEngine.Purchasing;
using Utills;
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
        [SerializeField] private UnityEngine.UI.Button restoreButton;
        [SerializeField] private IAPToggleController iapToggleController;

        [Space, Header("Toggles")] [SerializeField]
        private CustomToggle weeklyToggle;

        [SerializeField] private CustomToggle monthlyToggle;

        [Inject] private readonly IAPService _iapService;

        private void Start()
        {
            Init();
            _iapService.Initialize();
        }

        private void OnEnable()
        {
            _iapService.OnInitializeCompleted.AddListener(OnInitializeCompleted);
            _iapService.OnPurchaseCompleted.AddListener(OnPurchaseCompleted);
            _iapService.OnRestoreCompleted.AddListener(OnRestoreCompleted);
            _iapService.Initialize();
        }

        private void OnDisable()
        {
            _iapService.OnInitializeCompleted.RemoveListener(OnInitializeCompleted);
            _iapService.OnPurchaseCompleted.RemoveListener(OnPurchaseCompleted);
            _iapService.OnRestoreCompleted.RemoveListener(OnRestoreCompleted);
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

            if (restoreButton != null)
            {
                restoreButton.enabled = false;
            }
        }

        private void InitCommands()
        {
            privacyPolicyButton.onClick.AddListener(OnPrivacyClicked);
            termsConditionButton.onClick.AddListener(OnTermsClicked);
            restoreButton.onClick.AddListener(OnRestoreClicked);
        }

        public void CloseButton()
        {
            purchasePanelPage.SetActive(false);
            paidPanelPage.SetActive(false);
        }

        public void PurchaseButton()
        {
            if (weeklyToggle.IsSelected)
            {
                _iapService.Purchase(Constants.WeeklySubscriptionID);
                return;
            }

            if (monthlyToggle.IsSelected)
            {
                _iapService.Purchase(Constants.MonthlySubscriptionID);
                return;
            }
        }

        private void OnInitializeCompleted(Product[] products)
        {
            Debug.Log(
                $"IAPController: OnInitializeCompleted - {products[0].definition.id}, {products[1].definition.id}");

            if (_iapService.IsPurchased())
            {
                paidPanelPage.SetActive(true);
                purchasePanelPage.SetActive(false);
                return;
            }

            paidPanelPage.SetActive(false);
            purchasePanelPage.SetActive(true);

            if (purchaseButton != null)
            {
                purchaseButton.enabled = true;
            }

            if (restoreButton != null)
            {
                restoreButton.enabled = true;
            }

            if (iapToggleController != null)
            {
                iapToggleController.UpdateText(products);
            }
        }

        private void OnPurchaseCompleted()
        {
            Debug.Log("on purchase completed");
            paidPanelPage.SetActive(true);
            purchasePanelPage.SetActive(false);

            if (restoreButton != null)
            {
                restoreButton.enabled = false;
            }
        }

        private void OnRestoreCompleted()
        {
            if (_iapService.IsPurchased())
            {
                paidPanelPage.SetActive(true);
                purchasePanelPage.SetActive(false);
            }
            else
            {
                Toast.Show("You are not Subscribed!", 3f);
            }
        }

        private void OnPrivacyClicked()
        {
            Application.OpenURL(
                "https://doc-hosting.flycricket.io/arkidsdrawingapp/c7e33f88-dce1-40e4-9bb4-14ae65627321/privacy");
        }

        private void OnTermsClicked()
        {
            Application.OpenURL("https://doc-hosting.flycricket.io/terms/fcd9baaf-fa4d-41d7-b1b4-2b7e0795e318/terms");
        }

        private void OnRestoreClicked()
        {
            _iapService.Restore();
        }
    }
}