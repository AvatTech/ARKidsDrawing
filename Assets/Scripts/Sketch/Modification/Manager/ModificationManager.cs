using System;
using Sketch.Builder;
using Sketch.Modification.Abstraction;
using Sketch.Modification.Enum;
using Sketch.Modification.ModificationService;
using UI.Slider.Controller;
using UnityEngine;

namespace Sketch.Modification.Manager
{
    public class ModificationManager : MonoBehaviour
    {
        [SerializeField] private Sprite testSprite;
        [SerializeField] private SliderController mainSlider;


        public static ModificationManager Instance { get; private set; }

        private ModificationType _currentModificationType = ModificationType.Transparency;
        private ModifierService _modifierService = new ModifierService();
        private Model.Sketch _currentSketch;


        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            mainSlider.onValueChanged += f =>
            {
                Logger.Instance.InfoLog("On Value change in ModificationManager called!");
                Logger.Instance.InfoLog($"currentModificationtype is {_currentModificationType}");
                Logger.Instance.InfoLog($"Current sketch is {_currentSketch.Name}");

                switch (_currentModificationType)
                {
                    case ModificationType.Transparency:
                    {
                        _currentSketch.SetTransparency(f);
                        Logger.Instance.InfoLog(
                            $"transparency set is called!");

                        break;
                    }

                    case ModificationType.Scale:
                    {
                        _currentSketch.SetScale(f);
                        break;
                    }

                    case ModificationType.Rotation:
                    {
                        _currentSketch.SetRotation(f);
                        break;
                    }
                }
            };
        }


        public void SetModificationType(ModificationType modificationType)
        {
            _currentModificationType = modificationType;
        }

        // public void CreateNewSketchTest()
        // {
        //     _currentSketch = SketchBuilder
        //         .Builder()
        //         .SetName("A_alphabet")
        //         .SetSprite(testSprite)
        //         .SetTransparency(50)
        //         .SetScale(1)
        //         .SetRotation(0)
        //         .Build();
        // }


        public void OnSketchPlaced(GameObject placedSketchObj)
        {
            _currentSketch = placedSketchObj.GetComponentInChildren<Model.Sketch>();
            Logger.Instance.InfoLog("Object has been placed down!");
            if (_currentSketch is null)
            {
                Logger.Instance.InfoLog($"sketch is null!!!");
            }
            Logger.Instance.InfoLog($"Current sketch is {_currentSketch.Name}");
        }
    }
}