using System;
using System.Collections;
using System.Collections.Generic;
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