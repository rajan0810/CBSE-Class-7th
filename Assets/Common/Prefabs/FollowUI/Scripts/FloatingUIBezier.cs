using UnityEngine;

public class FloatingUIBezier : MonoBehaviour
{
    public Transform controllerTip;  // Assign the controller's proxy Rigidbody (ControllerTipRB)
    public LineRenderer lineRenderer;
    public int curveResolution = 30; // More points = smoother curve
    public float bendHeight1 = 0.4f; // Height for the first bend (1/4th distance)
    public float bendHeight2 = 0.2f; // Height for the second bend (closer to UI)

    void Update()
    {
        if (controllerTip != null && lineRenderer != null)
        {
            DrawBezierCurve();
        }
    }

    void DrawBezierCurve()
    {
        Vector3 start = controllerTip.position; // P0
        Vector3 end = transform.position; // P3

        // Define control points for a more dynamic bend
        Vector3 control1 = Vector3.Lerp(start, end, 0.25f) + Vector3.up * bendHeight1; // First bend at 1/4th distance
        Vector3 control2 = Vector3.Lerp(start, end, 0.75f) + Vector3.up * bendHeight2; // Second bend closer to UI

        lineRenderer.positionCount = curveResolution;

        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);

            // Quadratic Bezier equation with two control points
            Vector3 point = Mathf.Pow(1 - t, 3) * start + 
                            3 * Mathf.Pow(1 - t, 2) * t * control1 + 
                            3 * (1 - t) * Mathf.Pow(t, 2) * control2 + 
                            Mathf.Pow(t, 3) * end;

            lineRenderer.SetPosition(i, point);
        }
    }
}
