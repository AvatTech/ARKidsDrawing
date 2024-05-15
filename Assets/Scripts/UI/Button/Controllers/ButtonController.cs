using System;
using Sketches.Modification.Enum;
using Sketches.Modification.Manager;
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


        [Space, Header("Modification")] [SerializeField]
        private ModificationType modificationType = ModificationType.None;

        [SerializeField] private GameObject enablePanel;


        private UnityEngine.UI.Button _button;
        private TextMeshProUGUI _textMeshPro;

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

            enablePanel.SetActive(false);

            //_button.
        }

        private void setModificationType()
        {
            ModificationManager.Instance.SetModificationType(modificationType);
            restoreModificationValueOnSlider();
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
            enablePanel.SetActive(true);
            //_textMeshPro.CrossFadeAlpha(1, 0.2f, true);
            Debug.Log($"Button <{Label}> has been enabled.");
        }

        public void Disable()
        {
            enablePanel.SetActive(false);
            //_textMeshPro.CrossFadeAlpha(0.6f, 0.1f, true);
            Debug.Log($"Button <{Label}> has been disabled.");
        }
    }
}