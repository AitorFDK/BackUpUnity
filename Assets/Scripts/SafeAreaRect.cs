using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaRect : MonoBehaviour
{

    private RectTransform _rectTransform;
    private Rect _safeArea;
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _safeArea = Screen.safeArea;

        // Convert the safe area to local space
        Vector2 anchorMin = _safeArea.position / new Vector2(Screen.width, Screen.height);
        Vector2 anchorMax = (_safeArea.position + _safeArea.size) / new Vector2(Screen.width, Screen.height);

        _minAnchor = anchorMin;
        _maxAnchor = anchorMax;

        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        _rectTransform.anchorMin = _minAnchor;
        _rectTransform.anchorMax = _maxAnchor;
    }
}
