using UnityEngine;

public class BoundaryRespawner : MonoBehaviour
{
    // The spawn point for objects that leave the boundary
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving has a transform (it always will, but just in case)
        if (other != null)
        {
            RespawnObject(other.transform);
            Debug.Log("Okay I am working");
        }
    }

    private void RespawnObject(Transform objectToRespawn)
    {
        if (respawnPoint == null)
        {
            Debug.LogError("Respawn point is not assigned! Please assign a respawn point in the Inspector.");
            return; // Exit the function if the respawn point isn't assigned
        }

        // Log to confirm the respawn process
        Debug.Log($"Respawning object: {objectToRespawn.name}");

        // Move the object to the respawn point
        objectToRespawn.position = respawnPoint.position;

        // Reset Rigidbody velocity if the object has one
        Rigidbody rb = objectToRespawn.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Debug.Log($"Reset Rigidbody velocity for object: {objectToRespawn.name}");
        }
    }
}
