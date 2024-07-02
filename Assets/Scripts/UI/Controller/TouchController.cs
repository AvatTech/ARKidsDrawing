using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Controller
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider slider; 
        [SerializeField] private float minScaleFactor = 0.5f;
        [SerializeField] private float maxScaleFactor = 1.0f;
        
        private RectTransform _rectTransform;
        private Vector2 _prevTouchPos0, _prevTouchPos1;
        private bool _isScaling;


        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _isScaling = false;
        }

        private void Update()
        {
            if (IsPointerOverUIObject())
            {
                // If the touch is over the UI, do not move or scale the image
                return;
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Move
                var delta = Input.GetTouch(0).deltaPosition;
                _rectTransform.anchoredPosition += delta;
                _isScaling = false; // Reset scaling flag
            }
            else if (Input.touchCount == 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);

                var touch0Pos = touch0.position;
                var touch1Pos = touch1.position;

                if (!_isScaling)
                {
                    // Initialize the previous touch positions when starting a new scale operation
                    _prevTouchPos0 = touch0Pos;
                    _prevTouchPos1 = touch1Pos;
                    _isScaling = true;
                }

                if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    // Scale
                    var prevDistance = (_prevTouchPos0 - _prevTouchPos1).magnitude;
                    var currentDistance = (touch0Pos - touch1Pos).magnitude;
                    var scaleFactor = currentDistance / prevDistance;

                    // Calculate new scale
                    var newScale = _rectTransform.localScale * scaleFactor;

                    // Clamp the scale
                    newScale.x = Mathf.Clamp(newScale.x, minScaleFactor, maxScaleFactor);
                    newScale.y = Mathf.Clamp(newScale.y, minScaleFactor, maxScaleFactor);
                    newScale.z = Mathf.Clamp(newScale.z, minScaleFactor, maxScaleFactor);

                    _rectTransform.localScale = newScale;

                    // Rotation
                    var angle = Vector2.SignedAngle(_prevTouchPos1 - _prevTouchPos0, touch1Pos - touch0Pos);
                    _rectTransform.Rotate(Vector3.forward, angle);
                }

                // Update previous touch positions
                _prevTouchPos0 = touch0Pos;
                _prevTouchPos1 = touch1Pos;
            }
            else
            {
                // Reset scaling flag if no longer scaling
                _isScaling = false;
            }
        }

        private bool IsPointerOverUIObject()
        {
            // Check if the touch is over the slider UI element
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.touchCount > 0 ? (Vector2)Input.GetTouch(0).position : Vector2.zero;
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0 && results[0].gameObject == slider.gameObject;
        }
    }
}