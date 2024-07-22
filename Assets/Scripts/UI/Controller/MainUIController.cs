using System;
using Categories.Utills;
using Repositories;
using Services;
using UnityEngine;
using Utills;
using Zenject;

namespace UI.Controller
{
    public class MainUIController : MonoBehaviour
    {
        [Inject] private IAPRepository _iapRepository;
        [Inject] private IAPService _iapService;

        [Header("Panels")] [SerializeField] private GameObject categoryPanel;
        [SerializeField] private GameObject mainPagePanel;
        [SerializeField] private GameObject splashPagePanel;
        [SerializeField] private GameObject iapPagePanel;
        [SerializeField] private GameObject iapPurchasedPagePanel;
        [SerializeField] private GameObject iapBanner;

        private CurrentCategoryManager _currentCategoryManager;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            InitComponents();
            InitCommands();
        }

        private void InitComponents()
        {
            _currentCategoryManager = CurrentCategoryManager.Instance;
            iapBanner.SetActive(false);

            if (_currentCategoryManager != null && _currentCategoryManager.CurrentCategory != null)
            {
                ShowSketchesPanel();
            }

            CheckForPremium(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private void InitCommands()
        {
            _iapService.OnInitializeCompleted.AddListener(CheckForPremium);
            _iapService.Initialize();
        }

        private void OnDestroy()
        {
            PlayerPrefs.DeleteKey(Constants.KeyIsComingFromAR);
        }

        public void ShowSketchesPanel()
        {
            // mainPagePanel.SetActive(false);
            categoryPanel.SetActive(true);
        }


        public void ShowMainPage()
        {
            _currentCategoryManager = null;

            categoryPanel.SetActive(false);
            mainPagePanel.SetActive(true);
        }


        public void ShowSplashPage()
        {
            splashPagePanel.SetActive(true);
            categoryPanel.SetActive(false);
            mainPagePanel.SetActive(true);
        }

        public void ShowIAPPage()
        {
            var result = _iapRepository.IsPurchased();
            Debug.Log(result);
            if (result)
            {
                iapPagePanel.SetActive(true);
            }
            else
            {
                iapPurchasedPagePanel.SetActive(true);
            }
        }

        private void CheckForPremium(string arg0, string s, string s1, string arg3)
        {
            var result = _iapRepository.IsPurchased();
            if (result)
            {
                iapBanner.SetActive(false);
            }
            else
            {
                iapBanner.SetActive(true);
            }
        }
    }
}