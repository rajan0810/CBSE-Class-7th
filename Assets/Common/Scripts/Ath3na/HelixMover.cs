using UnityEngine;

public class HelixMover : MonoBehaviour
{
    [Header("Helix Settings")]
    [Tooltip("Radius of the helix path.")]
    public float radius = 5f;

    [Tooltip("Vertical distance between each loop (pitch).")]
    public float pitch = 2f;

    [Tooltip("Angular speed in radians per second.")]
    public float angularSpeed = 1f;

    private float angle; // Current angle in radians
    private Vector3 startPosition; // Stores the initial position
    private Quaternion startRotation; // Stores the initial rotation

    void Start()
    {
        // Store the object's initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Update the angle over time
        angle += angularSpeed * Time.deltaTime;

        // Calculate local helix movement (before applying rotation)
        float localX = radius * Mathf.Cos(angle);
        float localZ = radius * Mathf.Sin(angle);
        float localY = (pitch / (2 * Mathf.PI)) * angle; // Helix vertical motion

        // Create the local offset vector
        Vector3 localOffset = new Vector3(localX, localY, localZ);

        // Rotate the local offset based on the object's initial rotation
        Vector3 rotatedOffset = startRotation * localOffset;

        // Apply the new position relative to the start position
        Vector3 newPosition = startPosition + rotatedOffset;
        transform.position = newPosition;

        // Define the helix's central axis direction (using the object's initial up direction)
        Vector3 helixAxis = startRotation * Vector3.up;

        // Find the closest point on the helix axis from the current position.
        // This is done by projecting the offset (from the start) onto the axis.
        Vector3 offsetFromStart = newPosition - startPosition;
        float projectionLength = Vector3.Dot(offsetFromStart, helixAxis);
        Vector3 nearestPointOnAxis = startPosition + helixAxis * projectionLength;

        // Rotate the object so that its forward vector points toward the axis.
        // Using helixAxis as the world-up helps maintain consistency if the helix is rotated.
        transform.LookAt(nearestPointOnAxis, helixAxis);
    }
}
