using UnityEngine;

namespace UI.Toggle
{
    public class CustomToggleGroup : MonoBehaviour
    {
        private CustomToggle _lastSelectedToggle;
        private CustomToggle _currentSelectedToggle;

        private CustomToggle[] _toggles;

        private void Start()
        {
            _toggles = GetComponentsInChildren<CustomToggle>();
            foreach (var toggle in _toggles)
            {
                toggle.toggleGroup = this;
            }
        }

        public void SetCurrentlySelected(CustomToggle value)
        {
            if (_currentSelectedToggle != null)
            {
                if (_currentSelectedToggle != value)
                {
                    _currentSelectedToggle.SetSelected(false);
                    _lastSelectedToggle = _currentSelectedToggle;
                }
            }

            _currentSelectedToggle = value;
        }

        public void DeselectEverything()
        {
            if (_currentSelectedToggle != null)
            {
                _lastSelectedToggle = _currentSelectedToggle;
                _currentSelectedToggle = null;
            }

            foreach (var toggle in _toggles)
            {
                toggle.SetSelected(false);
            }
        }
    }
}