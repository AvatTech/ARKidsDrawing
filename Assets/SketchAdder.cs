using System;
using System.Collections;
using System.Collections.Generic;
using Sketches.Controller;
using UnityEngine;
using UnityEngine.UI;

public class SketchAdder : MonoBehaviour
{
    public RawImage sketchRawImage;

    private void Start()
    {
        sketchRawImage.texture = CurrentSketchHolder.Instance.CurrentSketchController.RawImage.texture;
    }
}