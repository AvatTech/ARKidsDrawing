using System;
using System.Threading.Tasks;
using Sketches.Model;
using Sketches.Services;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Sketches.Controller
{
    public class SketchController : MonoBehaviour
    {
        public Sketch Sketch { get; private set; }


        private RawImage _rawImage;


        private ImageLoaderService _imageLoaderService = new();


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _rawImage = GetComponentInChildren<RawImage>();
            SetScale(0.5f);
        }


        public void SetTransparency(float value)
        {
            _rawImage.color = new Color(1, 1, 1, value);
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


        public async Task SetImageFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            Debug.Log($"Loading sketch with url: {url}");
            var texture = await FetchImageFromUrl(url);

            await Task.Yield();

            _rawImage.texture = texture;
        }


        private async Task<Texture2D> FetchImageFromUrl(string url)
        {
            if (_imageLoaderService is null)
                throw new NullReferenceException("Image Loader Service is null!");

            return await _imageLoaderService!.TryGetTexture(url);
        }
    }
}