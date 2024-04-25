using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    [Header("Panels")] [SerializeField] private GameObject categoryPanel;
    [SerializeField] private GameObject mainPagePanel;


    public void ShowCategoryPanel()
    {
        mainPagePanel.SetActive(false);
        categoryPanel.SetActive(true);
    }


    public void ShowMainPage()
    {
        categoryPanel.SetActive(false);
        mainPagePanel.SetActive(true);
    }
}