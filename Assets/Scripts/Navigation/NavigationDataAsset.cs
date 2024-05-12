using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationDataAsset : MonoBehaviour
{
    public static NavigationDataAsset Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }


    [Space] public GameObject SplashScreenPanel;
    [Space] public GameObject MainScreenPanel;
    [Space] public GameObject SketchesScreenPanel;
}