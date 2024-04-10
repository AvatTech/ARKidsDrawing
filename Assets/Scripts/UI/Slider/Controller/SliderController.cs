using System;
using UnityEngine;

namespace UI.Slider.Controller
{
    public class SliderController : MonoBehaviour
    {
        private UnityEngine.UI.Slider slider;

        public Action<float> onValueChanged;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            slider = GetComponent<UnityEngine.UI.Slider>();
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }


        public void SetSliderValue(float value)
        {
            slider.value = Mathf.Clamp(value, 0f, 1f);
        }

        public void OnSliderValueChanged(float value)
        {
            onValueChanged.Invoke(value);
        }
    }
}