using System;
using System.Threading.Tasks;
using Repositories;
using Sketches.Model;
using Sketches.Utills;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using imageLoader = Extensions.Unity.ImageLoader.ImageLoader;

namespace Sketches.Controller
{
    public class SketchController : MonoBehaviour
    {
        public Sketch Sketch { get; set; }

        [SerializeField] private Image borderImage;
        private RawImage _rawImage;
        private Button _button;
        private bool _isDestroy;

        
        [SerializeField] private Sprite premiumSprite;


        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            _isDestroy = true;
        }

        private void OnSketchClicked()
        {
            CurrentSketchHolder.Instance.CurrentSketchUrl = Sketch.ImageUrl;

            // Load next scene
            //SceneManager.LoadScene("AppScene");
        }

        private void Init()
        {
            _rawImage = GetComponentInChildren<RawImage>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnSketchClicked);

            // set initial scale
            SetScale(0.5f);
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

            if (_isDestroy)
            {
                return;
            }

            // Debug.Log($"for {name}: texture: {texture == null} RawImage: {_rawImage == null}");

            if (_rawImage == null)
                _rawImage = GetComponentInChildren<RawImage>();

            _rawImage.texture = texture;
        }


        private async Task<Texture2D> FetchImageFromUrl(string url)
        {
            Texture2D t = null;

            await imageLoader.LoadSprite(url).Then((sprite => { t = sprite.texture; }));

            return t;
        }


        public void ConfigurePremium()
        {
            if (Sketch.IsPremium)
            {
                borderImage.sprite = premiumSprite;
            }
        }

        public void AddOnSketchClickedListener(UnityAction onSketchClicked)
        {
            if (_button is null)
                _button = GetComponent<Button>();
            
            _button.onClick.AddListener(onSketchClicked);
        }
        
        public void RemoveAllListeners()
        {
            if (_button is null)
                _button = GetComponent<Button>();
            
            _button.onClick.RemoveAllListeners();
        }
        
        
    }
}