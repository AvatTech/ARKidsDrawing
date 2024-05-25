using System;
using System.Threading.Tasks;
using Sketches.Model;
using Sketches.Services;
using Sketches.Utills;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using imageLoader = Extensions.Unity.ImageLoader.ImageLoader;

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

            var texture = await FetchImageFromUrl(url);
            
            RawImage.texture = texture;
        }


        private async Task<Texture2D> FetchImageFromUrl(string url)
        {
            Texture2D t = null;

            await imageLoader.LoadSprite(url).Then((sprite => { t = sprite.texture; }));

            return t;

        }
    }
}