using TMPro;
using UnityEngine;

namespace UI.Controller
{
    public class SplashScreenController : MonoBehaviour
    {
        public static SplashScreenController Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            SetState(SplashScreenState.Loading);
        }
    
    
        [SerializeField] private GameObject tryAgainButton;
        [SerializeField] private GameObject spinner;
        [SerializeField] private TextMeshProUGUI text;
    
    
    
        public void SetState(SplashScreenState splashScreenState)
        {
            switch (splashScreenState)
            {
                case SplashScreenState.Loading:
                {
                    gameObject.SetActive(true);
                    spinner.SetActive(true);
                    text.SetText("Loading...");
                    tryAgainButton.SetActive(false);
                    break;
                }

                case SplashScreenState.Failed:
                {
                    gameObject.SetActive(true);
                    spinner.SetActive(false);
                    text.SetText("Connect your device to the internet.");
                    tryAgainButton.SetActive(true);
                    break;
                }

                case SplashScreenState.Done:
                {
                    gameObject.SetActive(false);
                    spinner.SetActive(false);
                    tryAgainButton.SetActive(false);
                    break;
                }
            }
        }
    }


    public enum SplashScreenState
    {
        Loading,
        Failed,
        Done
    }
}