using UnityEngine;
using UnityEngine.UI;

public class SettingButtonController : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        button.onClick.AddListener(OnSettingsButton);
    }


    private void OnSettingsButton()
    {
        // load settings panel
        settingsPanel.SetActive(true);
    }
}