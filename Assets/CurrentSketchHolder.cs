using System;
using Sketches.Controller;
using UnityEngine;

public class CurrentSketchHolder : MonoBehaviour
{
    public static CurrentSketchHolder Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }


    public SketchController CurrentSketchController { get; set; }
}