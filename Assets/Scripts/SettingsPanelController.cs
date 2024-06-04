using Extensions.Unity.ImageLoader;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button privacyPolicyButton;
    [SerializeField] private Button termsConditionButton;
    [SerializeField] private Button rateButton;
    [SerializeField] private GameObject mainPanel;

    [Inject] private ReviewManager _reviewManager;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        backButton.onClick.AddListener(OnBackClicked);
        privacyPolicyButton.onClick.AddListener(OnPrivacyClicked);
        termsConditionButton.onClick.AddListener(OnTermsClicked);
        rateButton.onClick.AddListener(OnRateClicked);
    }

    private void OnBackClicked()
    {
        //mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnPrivacyClicked()
    {
        Application.OpenURL("https://doc-hosting.flycricket.io/arkidsdrawingapp/c7e33f88-dce1-40e4-9bb4-14ae65627321/privacy");
    }
    
    private void OnTermsClicked()
    {
        Application.OpenURL("https://doc-hosting.flycricket.io/terms/fcd9baaf-fa4d-41d7-b1b4-2b7e0795e318/terms");
    }
    
    private void OnRateClicked()
    {
        _reviewManager.Review();
        ImageLoader.ClearCache();
    }
}