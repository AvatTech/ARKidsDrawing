using System;
using Sketches.Modification.Enum;
using Sketches.Modification.Manager;
using TMPro;
using UnityEngine;

namespace UI.Button.Controllers
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ButtonController : MonoBehaviour
    {
        [field: SerializeField] public string Label { set; get; }

        [Space, Header("Modification")] [SerializeField]
        private ModificationType modificationType = ModificationType.None;

        [SerializeField] private GameObject enablePanel;


        [HideInInspector] public UnityEngine.UI.Button Button;
        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            Button = GetComponent<UnityEngine.UI.Button>();
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

            AddOnClick(setModificationType); // change current modification when clicked

            _textMeshPro.SetText(Label);
            enablePanel.SetActive(false);
        }

        public void setModificationType()
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
            Button.onClick.AddListener(onClick.Invoke);
        }


        public void Enable()
        {
            Button.Select();
            enablePanel.SetActive(true);
            Debug.Log($"Button <{Label}> has been enabled.");
        }

        public void Disable()
        {
            enablePanel.SetActive(false);
            Debug.Log($"Button <{Label}> has been disabled.");
        }
    }
}