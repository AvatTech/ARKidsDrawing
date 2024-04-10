using System;
using Sketch.Modification.Enum;
using Sketch.Modification.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Button.Controllers
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ButtonController : MonoBehaviour
    {
        [field: SerializeField] public string Label { set; get; }
        public UnityEvent OnClickEvent;

        private UnityEngine.UI.Button _button;
        private TextMeshProUGUI _textMeshPro;

        [Space, Header("Modification")] [SerializeField]
        private ModificationType modificationType = ModificationType.None;

        [Space, Header("ButtonSprites")] [SerializeField]
        private Sprite IdleSprite;

        [SerializeField] private Sprite EnableSprite;
        [SerializeField] private Sprite DisableSprite;


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _button = GetComponent<UnityEngine.UI.Button>();
            
            // Essential listeners for button
            _button.onClick.AddListener(OnClickEvent.Invoke);
            _button.onClick.AddListener(setModificationType); // change current modification when clicked


            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            _textMeshPro.SetText(Label);

        }

        private void setModificationType()
        {
            ModificationManager.Instance.SetModificationType(modificationType);
            restoreModificationValueOnSlider();
            Logger.Instance.InfoLog($"Current modification type is: {modificationType}.");
            
        }
        
        private void restoreModificationValueOnSlider()
        {
            ModificationManager.Instance.UpdateSliderValue();
        }

        public void AddOnClick(Action onClick)
        {
            OnClickEvent.AddListener(onClick.Invoke);
        }


        public void Enable()
        {
            _button.Select();
            Debug.Log($"Button <{Label}> has been enabled.");
        }

        public void Disable()
        {
            //_button.Select();
            Debug.Log($"Button <{Label}> has been disabled.");
        }
    }
}