using System;
using System.Threading.Tasks;
using Sketches.Model;
using Sketches.Services;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sketches.Controller
{
    public class SketchController : MonoBehaviour
    {
        public Sketch Sketch { get; private set; }


        public RawImage RawImage;
        private Button _button;


        private ImageLoaderService _imageLoaderService = new();


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            RawImage = GetComponentInChildren<RawImage>();
            _button = GetComponentInChildren<Button>();

            _button.onClick.AddListener(OnSketchClicked);

            SetScale(0.5f);
        }


        private void OnSketchClicked()
        {
            CurrentSketchHolder.Instance.CurrentSketchController = this;
            // Load next scene
            SceneManager.LoadScene("AppScene");
        }

        public void SetTransparency(float value)
        {
            RawImage.color = new Color(1, 1, 1, value);
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

            RawImage.texture = texture;
        }


        private async Task<Texture2D> FetchImageFromUrl(string url)
        {
            if (_imageLoaderService is null)
                throw new NullReferenceException("Image Loader Service is null!");

            return await _imageLoaderService!.TryGetTexture(url);
        }
    }
}