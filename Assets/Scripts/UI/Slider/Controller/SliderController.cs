using System;
using Sketches.Controller;
using UnityEngine;

namespace UI.Slider.Controller
{
    public class SliderController : MonoBehaviour
    {
        [SerializeField] private ARSketchController arSketchController;

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

        private void OnSliderValueChanged(float value)
        {
            // onValueChanged.Invoke(value);

            arSketchController.SetTransparency(Mathf.Abs(1 - value));
        }
    }
}