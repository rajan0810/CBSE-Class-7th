using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform playerCamera;
    public float rotationSpeed = 5f;
    // Update is called once per frame
    void Update()
    {
        if (playerCamera != null)
        {
            FacePlayer();
        }
    }
    void FacePlayer()
    {
        Vector3 directionToCamera = playerCamera.position - transform.position;
        directionToCamera.y = 0; // Keep rotation only on the Y-axis

        Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
