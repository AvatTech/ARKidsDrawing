using Sketches.Modification.Enum;
using UI.Slider.Controller;
using UnityEngine;

namespace Sketches.Modification.Manager
{
    public class ModificationManager : MonoBehaviour
    {
        [SerializeField] private SliderController mainSlider;
        [SerializeField] private ARSketchController _currentSketchController;

        public static ModificationManager Instance { get; private set; }


        [Space, Header("Default Values")] [SerializeField]
        private float defaultTransparency;

        [SerializeField] private float defaultRotation;
        [SerializeField] private float defaultScale;


        private ModificationType _currentModificationType = ModificationType.Transparency;

        // stored values!
        private float scaleSliderValue;
        private float rotationSliderValue;
        private float transparencySliderValue;


        private void Init()
        {
            scaleSliderValue = defaultScale;
            transparencySliderValue = defaultTransparency;
            rotationSliderValue = defaultRotation;

            _currentSketchController.SetScale(defaultScale);
            _currentSketchController.SetRotation(defaultRotation);
            _currentSketchController.SetTransparency(defaultTransparency);
        }

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(this);
            else
                Instance = this;
        }


        private void Start()
        {
            Init();

            mainSlider.onValueChanged += f =>
            {
                switch (_currentModificationType)
                {
                    case ModificationType.Transparency:
                    {
                        _currentSketchController.SetTransparency(f);
                        transparencySliderValue = f;
                        break;
                    }

                    case ModificationType.Scale:
                    {
                        _currentSketchController.SetScale(f);
                        scaleSliderValue = f;
                        break;
                    }

                    case ModificationType.Rotation:
                    {
                        _currentSketchController.SetRotation(f);
                        rotationSliderValue = f;
                        break;
                    }
                }
            };
        }


        public void UpdateSliderValue()
        {
            switch (_currentModificationType)
            {
                case ModificationType.Rotation:
                    mainSlider.SetSliderValue(rotationSliderValue);
                    break;


                case ModificationType.Scale:
                    mainSlider.SetSliderValue(scaleSliderValue);
                    break;

                case ModificationType.Transparency:
                    mainSlider.SetSliderValue(transparencySliderValue);
                    break;
            }
        }

        public void SetModificationType(ModificationType modificationType)
        {
            _currentModificationType = modificationType;
        }

        public void OnSketchPlaced(GameObject placedSketchObj)
        {
            _currentSketchController = placedSketchObj.GetComponentInChildren<ARSketchController>();
        }
    }
}