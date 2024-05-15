using UnityEngine;

namespace Loading
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private GameObject spinner;
        [SerializeField] private GameObject backgroundPanel;

        public static LoadingController Instance { get; set; }


        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            Hide();
        }


        public void Show()
        {
            spinner.SetActive(true);
            backgroundPanel.SetActive(true);
        }

        public void Hide()
        {
            spinner.SetActive(false);
            backgroundPanel.SetActive(false);
        }
    }
}