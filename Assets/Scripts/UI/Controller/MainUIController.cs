using Categories.Utills;
using Repositories;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class MainUIController : MonoBehaviour
    {
        [Inject] private IAPRepository _iapRepository;
        
        [Header("Panels")] [SerializeField] private GameObject categoryPanel;
        [SerializeField] private GameObject mainPagePanel;
        [SerializeField] private GameObject splashPagePanel;
        [SerializeField] private GameObject iapPagePanel;
        [SerializeField] private GameObject iapPurchasedPagePanel;
        [SerializeField] private GameObject iapBanner;

        private CurrentCategoryManager _currentCategoryManager;

        private void Awake()
        {
            _currentCategoryManager = CurrentCategoryManager.Instance;
            iapBanner.SetActive(false);
        }

        private void Start()
        {
            if (_currentCategoryManager != null && _currentCategoryManager.CurrentCategory != null)
            {
                ShowSketchesPanel();
            }

            CheckForPremium();
        }

        public void ShowSketchesPanel()
        {
            mainPagePanel.SetActive(false);
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

        private void CheckForPremium()
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