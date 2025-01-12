using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem waterParticleSystem; // Assign your Particle System in the Inspector

    [SerializeField]
    private Transform sprinklerTransform; // Assign the Transform of the sprinkler in the Inspector

    [SerializeField]
    private float minRotationX = 30f; // Minimum x rotation to activate the Particle System
    [SerializeField]
    private float maxRotationX = 50f; // Maximum x rotation to activate the Particle System

    private bool isSprinkling = false; // Tracks the current state of the Particle System

    private void Update()
    {
        if (sprinklerTransform == null || waterParticleSystem == null)
        {
            Debug.LogWarning("Sprinkler Transform or Water Particle System is not assigned!");
            return;
        }

        // Get the current x-axis rotation
        float currentRotationX = sprinklerTransform.eulerAngles.x;

        // Normalize the rotation to handle Unity's angle wrapping (0 to 360 degrees)
        currentRotationX = currentRotationX > 180f ? currentRotationX - 360f : currentRotationX;

        // Check if the x rotation is within the desired range
        if (currentRotationX >= minRotationX && currentRotationX <= maxRotationX)
        {
            if (!isSprinkling)
            {
                StartSprinkling();
            }
        }
        else
        {
            if (isSprinkling)
            {
                StopSprinkling();
            }
        }
    }

    private void StartSprinkling()
    {
        waterParticleSystem.Play();
        isSprinkling = true;
        Debug.Log("Water sprinkler activated.");
    }

    private void StopSprinkling()
    {
        waterParticleSystem.Stop();
        isSprinkling = false;
        Debug.Log("Water sprinkler deactivated.");
    }
}
