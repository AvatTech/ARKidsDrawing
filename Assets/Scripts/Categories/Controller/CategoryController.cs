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
            _image = GetComponentInChildren<RawImage>();
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            _button = GetComponent<Button>();
            currentCategoryManager = CurrentCategoryManager.Instance;
            _mainUIController = FindObjectOfType<MainUIController>();

            _button.onClick.AddListener(OnClick);
        }


        public void OnClick()
        {
            currentCategoryManager.CurrentCategory = Category;
            _mainUIController.ShowSketchesPanel();
            Debug.Log($"Current Category is {Category.Name}");
        }

        public async Task SetImageFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            var texture = await FetchImageFromUrl(url);

            await Task.Yield();

            _image.texture = texture;
        }

        public void SetText(string text)
        {
            _textMeshPro.SetText(text);
        }

        private async Task<Texture2D> FetchImageFromUrl(string url)
        {
            if (_imageLoaderService is null)
            {
                Debug.Log("image loader is null");
            }

            return await _imageLoaderService!.TryGetTexture(url);
        }
    }
}