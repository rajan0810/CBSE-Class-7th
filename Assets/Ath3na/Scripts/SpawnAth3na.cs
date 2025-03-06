using UnityEngine;
using System.Collections;

public class SpawnAth3na : MonoBehaviour
{
    public Material customMaterial; // Assign this in the inspector
    public float transitionSpeed = 1f; // Speed of transition
    private Coroutine transitionCoroutine;

    void Start()
    {
        if (customMaterial != null)
        {
            // Ensure initial state is despawned
            customMaterial.SetFloat("_spawn_y", 1f);
        }
    }

    public void SpawnObject()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(ChangeSpawnY(0f)); // Gradually change spawn_y to 0
    }

    public void DespawnObject()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(ChangeSpawnY(1f)); // Gradually change spawn_y to 1
    }

    private IEnumerator ChangeSpawnY(float targetValue)
    {
        float currentValue = customMaterial.GetFloat("_spawn_y");
        while (Mathf.Abs(currentValue - targetValue) > 0.01f)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * transitionSpeed);
            customMaterial.SetFloat("_spawn_y", currentValue);
            yield return null;
        }
        customMaterial.SetFloat("_spawn_y", targetValue); // Ensure it reaches exact target value
    }
}
