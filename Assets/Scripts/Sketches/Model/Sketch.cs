using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Sketches.Model
{
    public class Sketch : MonoBehaviour
    {
        public string Name;

        private Sprite _sprite;
        private Image _image;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _image = GetComponentInChildren<Image>();
            _sprite = _image.sprite;
            SetScale(0.5f);
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
            transform.rotation = quaternion.RotateY(value * 10);
            //transform.Rotate(0f, value * 2f, 0f);
        }

        public void SetScale(float value)
        {
            var targetValue = Mathf.Lerp(0.5f, 1.5f, value);

            transform.localScale = new Vector3(targetValue, targetValue, targetValue);
        }
    }
}