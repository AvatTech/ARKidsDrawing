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
            _button.onClick.AddListener(OnClickEvent.Invoke);

            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            _textMeshPro.SetText(Label);

            var modificationManager = ModificationManager.Instance;

            //todo: Change this test code later
            // switch (Label)
            // {
            //     case "Rotation":
            //         modificationManager.SetModificationType(ModificationType.Rotation);
            //         break;
            //
            //     case "Scale":
            //         modificationManager.SetModificationType(ModificationType.Scale);
            //         break;
            //
            //     case "Transparency":
            //         modificationManager.SetModificationType(ModificationType.Transparency);
            //         break;
            //
            //     default:
            //         throw new Exception("Button Label not found!");
            // }
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