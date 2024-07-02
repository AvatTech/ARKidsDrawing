using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Controller
{
    public class TouchControls : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Slider slider;
        [SerializeField] private float minScaleFactor = 0.5f;
        [SerializeField] private float maxScaleFactor = 1.0f;

        private RectTransform _rectTransform;
        private Vector2 _prevTouchPos0, _prevTouchPos1;
        private bool _isScaling;
        private bool _isInteractingWithSlider;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _isScaling = false;
            _isInteractingWithSlider = false;

            // Add event triggers to detect when the slider interaction starts and ends
            var sliderEventTrigger = slider.gameObject.AddComponent<EventTrigger>();

            var pointerDownEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDownEntry.callback.AddListener((data) => { _isInteractingWithSlider = true; });
            sliderEventTrigger.triggers.Add(pointerDownEntry);

            var pointerUpEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUpEntry.callback.AddListener((data) => { _isInteractingWithSlider = false; });
            sliderEventTrigger.triggers.Add(pointerUpEntry);
        }

        private void Update()
        {
            if (_isInteractingWithSlider)
            {
                // If the user is interacting with the slider, do not move or scale the image
                return;
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Move
                var delta = Input.GetTouch(0).deltaPosition;
                _rectTransform.anchoredPosition += delta;
                ClampPosition();
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
                    ClampPosition();

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

        private void ClampPosition()
        {
            var corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);

            float screenLeft = 0;
            float screenRight = Screen.width;
            float screenBottom = 0;
            float screenTop = Screen.height;

            var imageWidth = corners[2].x - corners[0].x;
            var imageHeight = corners[2].y - corners[0].y;

            var halfImageWidth = imageWidth / 2;
            var halfImageHeight = imageHeight / 2;

            var clampedPosition = _rectTransform.anchoredPosition;

            if (corners[0].x > screenRight - halfImageWidth)
            {
                clampedPosition.x -= corners[0].x - (screenRight - halfImageWidth);
            }
            else if (corners[2].x < screenLeft + halfImageWidth)
            {
                clampedPosition.x += (screenLeft + halfImageWidth) - corners[2].x;
            }

            if (corners[0].y > screenTop - halfImageHeight)
            {
                clampedPosition.y -= corners[0].y - (screenTop - halfImageHeight);
            }
            else if (corners[2].y < screenBottom + halfImageHeight)
            {
                clampedPosition.y += (screenBottom + halfImageHeight) - corners[2].y;
            }

            _rectTransform.anchoredPosition = clampedPosition;
        }
    }
}