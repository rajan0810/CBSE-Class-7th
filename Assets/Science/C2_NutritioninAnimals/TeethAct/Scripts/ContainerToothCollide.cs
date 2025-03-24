using UnityEngine;

public class ContainerToothCollide : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
    }
}
