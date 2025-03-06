using UnityEngine;

public class HelixParticleFollow : MonoBehaviour
{
    public Transform target;  // The character to orbit around
    public ParticleSystem particleSystem;
    public float radius = 2f; // Base radius of the helix
    public float speed = 3f;  // Rotation speed
    public float pitch = 1f;  // Vertical gap between helix loops
    public float randomness = 0.5f; // Randomness factor for natural movement
    public float maxHeight = 5f; // Max height cap for particle deletion

    private ParticleSystem.Particle[] particles;
    private float[] randomOffsets; // Stores a random offset per particle
    private float[] randomRadii;   // Stores random radius variations per particle

    void Start()
    {
        if (particleSystem == null) return;

        int maxParticles = particleSystem.main.maxParticles;
        randomOffsets = new float[maxParticles];
        randomRadii = new float[maxParticles];

        for (int i = 0; i < maxParticles; i++)
        {
            randomOffsets[i] = Random.Range(0f, Mathf.PI * 2); // Unique angle offset
            randomRadii[i] = radius + Random.Range(-randomness, randomness); // Vary the radius slightly
        }
    }

    void LateUpdate()
    {
        if (particleSystem == null || target == null) return;

        int numParticlesAlive = particleSystem.particleCount;
        if (particles == null || particles.Length < numParticlesAlive)
        {
            particles = new ParticleSystem.Particle[numParticlesAlive];
        }

        particleSystem.GetParticles(particles);

        int validParticleCount = 0;

        for (int i = 0; i < numParticlesAlive; i++)
        {
            float theta = (Time.time * speed + randomOffsets[i]); // Angle based on time and randomness
            float particleRadius = randomRadii[i]; // Apply unique radius variation

            float x = Mathf.Cos(theta) * particleRadius;
            float z = Mathf.Sin(theta) * particleRadius;
            float y = (pitch / (2 * Mathf.PI)) * theta; // Smooth vertical progression

            // Delete particle if it goes beyond maxHeight
            if (y > maxHeight)
            {
                continue; // Skip this particle (removes it from the list)
            }

            particles[validParticleCount].position = target.position + new Vector3(x, y, z);
            particles[validParticleCount].remainingLifetime = particles[i].remainingLifetime; // Maintain original lifetime
            validParticleCount++; // Only keep valid particles
        }

        // Apply only the remaining valid particles
        particleSystem.SetParticles(particles, validParticleCount);
    }
}
