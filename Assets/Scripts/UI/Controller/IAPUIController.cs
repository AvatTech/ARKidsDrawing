using Services;
using UI.Toggle;
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

            _iapService.OnInitializeCompleted.AddListener(OnInitializeCompleted);
            _iapService.OnPurchaseCompleted.AddListener(OnPurchaseCompleted);
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
                _iapService.Purchase(ProductType.SubscriptionWeekly);
                return;
            }

            if (monthlyToggle.IsSelected)
            {
                _iapService.Purchase(ProductType.SubscriptionMonthly);
                return;
            }
        }

        private void OnInitializeCompleted(string weeklyTitle, string weeklyPrice, string monthlyTitle,
            string monthlyPrice)
        {
            Debug.Log("on initialize completed");

            Debug.Log($"{weeklyTitle} {weeklyPrice} {monthlyTitle} {monthlyPrice}");

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
                iapToggleController.UpdateText(
                    weeklyTitle,
                    weeklyPrice,
                    monthlyTitle,
                    monthlyPrice);
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
            var result = _iapService.Restore();

            paidPanelPage.SetActive(result);
            purchasePanelPage.SetActive(!result);
        }
    }
}