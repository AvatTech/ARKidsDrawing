using UnityEngine;

namespace UI.Controller
{
    public class TouchController : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Vector2 prevTouchPos0, prevTouchPos1;
        private bool isScaling;

        // Minimum and maximum scale factors
        [SerializeField] private float minScaleFactor = 0.5f;
        [SerializeField] private float maxScaleFactor = 1.0f;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            isScaling = false;
        }

        private void Update()
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Move
                var delta = Input.GetTouch(0).deltaPosition;
                rectTransform.anchoredPosition += delta;
                isScaling = false; // Reset scaling flag
            }
            else if (Input.touchCount == 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);

                var touch0Pos = touch0.position;
                var touch1Pos = touch1.position;

                if (!isScaling)
                {
                    // Initialize the previous touch positions when starting a new scale operation
                    prevTouchPos0 = touch0Pos;
                    prevTouchPos1 = touch1Pos;
                    isScaling = true;
                }

                if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    // Scale
                    var prevDistance = (prevTouchPos0 - prevTouchPos1).magnitude;
                    var currentDistance = (touch0Pos - touch1Pos).magnitude;
                    var scaleFactor = currentDistance / prevDistance;

                    // Calculate new scale
                    var newScale = rectTransform.localScale * scaleFactor;

                    // Clamp the scale
                    newScale.x = Mathf.Clamp(newScale.x, minScaleFactor, maxScaleFactor);
                    newScale.y = Mathf.Clamp(newScale.y, minScaleFactor, maxScaleFactor);
                    newScale.z = Mathf.Clamp(newScale.z, minScaleFactor, maxScaleFactor);

                    rectTransform.localScale = newScale;

                    // Rotation
                    var angle = Vector2.SignedAngle(prevTouchPos1 - prevTouchPos0, touch1Pos - touch0Pos);
                    rectTransform.Rotate(Vector3.forward, angle);
                }

                // Update previous touch positions
                prevTouchPos0 = touch0Pos;
                prevTouchPos1 = touch1Pos;
            }
            else
            {
                // Reset scaling flag if no longer scaling
                isScaling = false;
            }
        }
    }
}