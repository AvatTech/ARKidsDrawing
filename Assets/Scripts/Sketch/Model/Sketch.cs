using UnityEngine;
using UnityEngine.UI;

namespace Sketch.Model
{
    public class Sketch : MonoBehaviour
    {
        public string Name;

        private Sprite _sprite;
        private Image _image;

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _sprite = _image.sprite;
        }

        public void SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            _image.sprite = _sprite;
        }

        public void SetTransparency(float value)
        {
            _image.color = new Color(1, 1, 1, value);
            
        }

        public void SetRotation(float value)
        {
            //transform.Rotate();
        }

        public void SetScale(float value)
        {
            //transform.Rotate();
        }
    }
}