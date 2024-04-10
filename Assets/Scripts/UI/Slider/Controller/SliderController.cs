using System;
using UnityEngine;

namespace UI.Slider.Controller
{
    public class SliderController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider slider;

        public Action<float> onValueChanged;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }


        public void OnSliderValueChanged(float value)
        {
            onValueChanged.Invoke(value);
        }
    }
}