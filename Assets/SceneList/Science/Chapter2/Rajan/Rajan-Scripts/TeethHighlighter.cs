using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethHighlighter : MonoBehaviour
{
    [Header("Assign Tooth GameObjects Here")]
    public List<GameObject> teethToHighlight;

    [Header("Highlight Settings")]
    public Material outlineMaterial; // Shader material for outline effect
    public float highlightDuration = 2f; // Duration for which the highlight remains

    private Dictionary<GameObject, List<Material>> originalMaterials = new Dictionary<GameObject, List<Material>>();

    void Start()
    {
        // Store original materials of each tooth
        foreach (GameObject tooth in teethToHighlight)
        {
            if (tooth != null)
            {
                SkinnedMeshRenderer renderer = tooth.GetComponent<SkinnedMeshRenderer>();
                if (renderer != null)
                {
                    originalMaterials[tooth] = new List<Material>(renderer.materials);
                }
            }
        }
    }

    public IEnumerator HighlightTeeth()
    {
        foreach (GameObject tooth in teethToHighlight)
        {
            if (tooth != null)
            {
                SkinnedMeshRenderer renderer = tooth.GetComponent<SkinnedMeshRenderer>();
                if (renderer != null)
                {
                    List<Material> newMaterials = new List<Material>(renderer.materials);
                    newMaterials.Add(outlineMaterial);
                    renderer.materials = newMaterials.ToArray();
                }
            }
        }
        yield return new WaitForSeconds(highlightDuration);
        RemoveHighlight();
    }

    public void RemoveHighlight()
    {
        foreach (GameObject tooth in teethToHighlight)
        {
            if (tooth != null && originalMaterials.ContainsKey(tooth))
            {
                SkinnedMeshRenderer renderer = tooth.GetComponent<SkinnedMeshRenderer>();
                if (renderer != null)
                {
                    renderer.materials = originalMaterials[tooth].ToArray();
                    Debug.Log("Highlight removed from tooth: " + tooth.name);
                }
            }
        }
    }
}