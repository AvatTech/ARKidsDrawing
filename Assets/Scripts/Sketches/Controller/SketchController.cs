using System.Threading.Tasks;
using Sketches.Model;
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
        public Sketch Sketch { get; set; }


        private RawImage _rawImage;
        private Button _button;


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _rawImage = GetComponentInChildren<RawImage>();
            _button = GetComponentInChildren<Button>();

            _button.onClick.AddListener(OnSketchClicked);

            SetScale(0.5f);
        }


        private void OnSketchClicked()
        {
            CurrentSketchHolder.Instance.CurrentSketchUrl = Sketch.ImageUrl;

            // Load next scene
            SceneManager.LoadScene("AppScene");
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

            Debug.Log($"for {name}: texture: {texture == null} RawImage: {_rawImage == null}");

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
    }
}