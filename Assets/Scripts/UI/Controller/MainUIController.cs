using Categories.Utills;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Controller
{
    public class MainUIController : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private GameObject categoryPanel;
        [SerializeField] private GameObject mainPagePanel;
        [SerializeField] private GameObject splashPagePanel;
        [SerializeField] private GameObject iapPagePanel;

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
            iapPagePanel.SetActive(true);
        }
    }
}