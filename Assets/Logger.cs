using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Logger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logTextMeshPro;

    private string logText { get; set; }


    public static Logger Instance { private set; get; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        logText = "[LOGGER]\n";
    }

    public void InfoLog(string text)
    {
        logText += "-" + text + "\n";
        UpdateUI();
    }

    private void UpdateUI()
    {
        logTextMeshPro.SetText(logText);
    }
}