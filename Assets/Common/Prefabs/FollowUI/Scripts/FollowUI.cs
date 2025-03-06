using UnityEngine;

public class FollowUI : MonoBehaviour
{
    public GameObject point1;
    public GameObject point2;
    public GameObject UI; // Reference to the UI
    public LineRenderer lineRenderer;

    private DynamicBezierLine _DynamicBezierLine;
    private UIEffectsController _UIEffectsController;

    void Start()
    {
        // Ensure LineRenderer and UI references are set correctly
        if (lineRenderer != null)
        {
            _DynamicBezierLine = lineRenderer.GetComponent<DynamicBezierLine>();
        }

        if (UI != null)
        {
            _UIEffectsController = UI.GetComponent<UIEffectsController>();
        }
    }

    public void EnableFollowUI()
    {
        if (point1 != null) point1.SetActive(true);
        if (lineRenderer != null) lineRenderer.gameObject.SetActive(true); // Fixed LineRenderer activation
        if (point2 != null) point2.SetActive(true);
    }

    public void DisableFollowUI()
    {
        // Disable UI effects if available
        if (_UIEffectsController != null)
        {
            _UIEffectsController.DisableUIWithAnimation(); // Fixed function call
        }

        // Disable the dynamic line animation if available
        if (_DynamicBezierLine != null)
        {
            _DynamicBezierLine.DisableLineAnimation();
        }

        // Optionally, disable the points if needed
        if (point1 != null) point1.SetActive(false);
        if (point2 != null) point2.SetActive(false);
    }
}
