using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Toggle
{
    public class CustomToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image targetImage;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite highlightedSprite;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite disabledSprite;
        [SerializeField] private UnityEvent onSelect;

        [HideInInspector] public CustomToggleGroup toggleGroup;

        private bool _isSelected;
        public bool IsSelected => _isSelected;

        private bool _isDisabled;
        public bool IsDisabled => _isDisabled;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isDisabled)
                return;

            if (_isSelected)
            {
                OnDeselect();
            }
            else
            {
                onSelect?.Invoke();
                SetSelected(true);
            }
        }

        public void OnDeselect()
        {
            if (_isDisabled)
                return;

            SetSelected(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isDisabled)
                return;

            if (!_isSelected)
            {
                SetSprite(highlightedSprite);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isDisabled)
                return;

            if (!_isSelected)
            {
                SetSprite(normalSprite);
            }
        }

        public void SetSelected(bool value)
        {
            if (_isDisabled)
                return;

            _isSelected = value;
            if (_isSelected)
            {
                if (toggleGroup)
                {
                    toggleGroup.SetCurrentlySelected(this);
                }

                SetSprite(selectedSprite);
            }
            else
            {
                SetSprite(normalSprite);
            }
        }

        public void SetDisabled(bool value)
        {
            _isDisabled = value;
            if (_isDisabled)
            {
                _isSelected = false;
                SetSprite(disabledSprite);
            }
            else
            {
                SetSprite(normalSprite);
            }
        }

        private void SetSprite(Sprite sprite)
        {
            if (targetImage != null)
            {
                targetImage.sprite = sprite;
            }
        }
    }
}