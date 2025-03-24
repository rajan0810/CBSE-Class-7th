using System.Collections;
using UnityEngine;

public class OrganHighlighter : MonoBehaviour
{
    public Material highlightMaterial; // Additional material for highlighting
    public float highlightDuration = 2f; // Public duration input

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterials = meshRenderer.materials; // Store original materials
        }
    }

    public IEnumerator HighlightOrgan()
    {
        if (meshRenderer != null && highlightMaterial != null)
        {
            // Create a new array with an extra slot for the outline material
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(newMaterials, 0);
            newMaterials[newMaterials.Length - 1] = highlightMaterial; // Add highlight material

            meshRenderer.materials = newMaterials; // Apply new materials list

            yield return new WaitForSeconds(highlightDuration); // Wait for public duration

            meshRenderer.materials = originalMaterials; // Revert back to original materials
        }
    }
}
