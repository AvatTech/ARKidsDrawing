using System;
using System.Threading.Tasks;
using Categories.Model;
using Categories.Utills;
using Sketches.Services;
using TMPro;
using UI.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Categories.Controller
{
    public class CategoryController : MonoBehaviour
    {
        public Category Category { get; set; }


        private RawImage _image;
        private TextMeshProUGUI _textMeshPro;
        private Button _button;

        private ImageLoaderService _imageLoaderService = new();
        private CurrentCategoryManager currentCategoryManager;
        private MainUIController _mainUIController;


        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            
        }

        private void Init()
        {
            InitComponents();
        }

        private void InitComponents()
        {
            _image = GetComponentInChildren<RawImage>();

            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

            _button = GetComponent<Button>();

            currentCategoryManager = CurrentCategoryManager.Instance;

            _mainUIController = FindObjectOfType<MainUIController>();

            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            currentCategoryManager.CurrentCategory = Category;

            _mainUIController.ShowSketchesPanel();
        }

        public async Task SetImageFromUrl(string url, Action<Exception> onFailed)
        {
            if (string.IsNullOrEmpty(url)) return;

            var texture = await FetchImageFromUrl(url, onFailed);

            await Task.Yield();

            _image.texture = texture;
        }

        public void SetText(string text)
        {
            _textMeshPro.SetText(text);
        }

        private async Task<Texture2D> FetchImageFromUrl(string url, Action<Exception> onFailed)
        {
            return await _imageLoaderService!.TryGetTexture(url, onFailed);
        }
    }
}