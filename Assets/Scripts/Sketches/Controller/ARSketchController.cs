using System;
using Extensions.Unity.ImageLoader;
using Sketches.Utills;
using UnityEngine;
using UnityEngine.UI;

namespace Sketches.Controller
{
    public class ARSketchController : MonoBehaviour
    {
        private RawImage _rawImage;

        [SerializeField] private RectTransform parent;

        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;


        [Space, SerializeField] private float minRotation;
        [SerializeField] private float maxRotation;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }

        private async void OnEnable()
        {
            try
            {
                //_sketchController = CurrentSketchHolder.Instance.CurrentSketchUrl;

                await ImageLoader.LoadSprite(CurrentSketchHolder.Instance.CurrentSketchUrl).ThenSet(_rawImage);
            }
            catch (Exception e)
            {
                //LoggerScript.Instance.PrintLog(e.Message);
                //todo: Implement exception handling
            }
        }

        public void SetTransparency(float value)
        {
            _rawImage.color = new Color(1, 1, 1, value);
        }

        public void SetRotation(float value)
        {
            // Calculate the rotation based on the input
            var rotationZ = value * 360f;

            // Apply the rotation to the GameObject
            parent.rotation = Quaternion.Euler(0, 0, rotationZ);
        }

        public void SetScale(float value)
        {
            var targetValue = Mathf.Lerp(minScale, maxScale, value);
            parent.localScale = new Vector3(targetValue, targetValue, targetValue);
        }
    }
}