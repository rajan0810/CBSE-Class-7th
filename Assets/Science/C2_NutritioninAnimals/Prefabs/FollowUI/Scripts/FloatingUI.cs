using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public Transform controllerTip;  // Assign the controller tip
    public LineRenderer lineRenderer;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
    }

    void Update()
    {
        if (controllerTip != null)
        {
            // Keep the line renderer updated
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, controllerTip.position);
                lineRenderer.SetPosition(1, transform.position);
            }
        }
    }
}
