using UnityEngine;

namespace UI.Controller
{
    public class TouchController : MonoBehaviour
    {
    private RectTransform rectTransform;
    private Vector2 prevTouchPos0, prevTouchPos1;
    private bool isScaling;

    // Minimum and maximum scale factors
    public float minScaleFactor = 0.5f;
    public float maxScaleFactor = 1.0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isScaling = false;
    }

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Move
            Vector2 delta = Input.GetTouch(0).deltaPosition;
            rectTransform.anchoredPosition += delta;
            isScaling = false; // Reset scaling flag
        }
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Pos = touch0.position;
            Vector2 touch1Pos = touch1.position;

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
                float prevDistance = (prevTouchPos0 - prevTouchPos1).magnitude;
                float currentDistance = (touch0Pos - touch1Pos).magnitude;
                float scaleFactor = currentDistance / prevDistance;

                // Calculate new scale
                Vector3 newScale = rectTransform.localScale * scaleFactor;

                // Clamp the scale
                newScale.x = Mathf.Clamp(newScale.x, minScaleFactor, maxScaleFactor);
                newScale.y = Mathf.Clamp(newScale.y, minScaleFactor, maxScaleFactor);
                newScale.z = Mathf.Clamp(newScale.z, minScaleFactor, maxScaleFactor);

                rectTransform.localScale = newScale;

                // Rotation
                float angle = Vector2.SignedAngle(prevTouchPos1 - prevTouchPos0, touch1Pos - touch0Pos);
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