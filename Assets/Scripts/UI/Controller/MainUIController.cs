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

        private CurrentCategoryManager _currentCategoryManager;

        private void Awake()
        {
            _currentCategoryManager = CurrentCategoryManager.Instance;
        }

        private void Start()
        {
            if (_currentCategoryManager != null && _currentCategoryManager.CurrentCategory != null)
            {
                ShowSketchesPanel();
            }
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
            var result = _iapRepository.IsPurchased(false);
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
    }
}