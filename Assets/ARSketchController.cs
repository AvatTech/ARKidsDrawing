using System;
using System.Collections;
using System.Collections.Generic;
using Sketches.Controller;
using Sketches.Utills;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ARSketchController : MonoBehaviour
{
    private SketchController _sketchController;
    private RawImage _rawImageComponent;

    private void Awake()
    {
        _rawImageComponent = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        try
        {
            _sketchController = CurrentSketchHolder.Instance.CurrentSketchController;
            _rawImageComponent.texture = _sketchController.RawImage.texture;
        }
        catch (Exception e)
        {
            LoggerScript.Instance.PrintLog(e.Message);
        }
    }

    public void SetTransparency(float value)
    {
        _sketchController.RawImage.color = new Color(1, 1, 1, value);
    }

    public void SetRotation(float value)
    {
        transform.rotation = quaternion.RotateY(value * 10);
    }

    public void SetScale(float value)
    {
        var targetValue = Mathf.Lerp(0.5f, 1.5f, value);
        transform.localScale = new Vector3(targetValue, targetValue, targetValue);
    }
}