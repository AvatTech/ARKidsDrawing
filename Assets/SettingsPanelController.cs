using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainPanel;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
    }


    private void OnBackClicked()
    {
        //mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}