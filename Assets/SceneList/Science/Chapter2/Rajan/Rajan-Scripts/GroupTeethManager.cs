using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTeethManager : MonoBehaviour
{
    [Header("Tooth Groups")]
    public GroupTeethHighlighter incisorsHighlighter;
    public GroupTeethHighlighter caninesHighlighter;
    public GroupTeethHighlighter premolarsHighlighter;
    public GroupTeethHighlighter molarsHighlighter;

    [Header("Delay Between Highlights")]
    public float delayBetweenGroups = 1f; // Extra delay between each group

    void Start()
    {
        StartCoroutine(HighlightTeethSequence());
    }

    private IEnumerator HighlightTeethSequence()
    {
        yield return StartCoroutine(incisorsHighlighter.HighlightTeeth());
        // incisorsHighlighter.RemoveHighlight();
        Debug.Log("Incisors highlight removed");
        yield return new WaitForSeconds(delayBetweenGroups);

        yield return StartCoroutine(caninesHighlighter.HighlightTeeth());
        // caninesHighlighter.RemoveHighlight();
        Debug.Log("Canines highlight removed");
        yield return new WaitForSeconds(delayBetweenGroups);

        yield return StartCoroutine(premolarsHighlighter.HighlightTeeth());
        // premolarsHighlighter.RemoveHighlight();
        Debug.Log("Premolars highlight removed");
        yield return new WaitForSeconds(delayBetweenGroups);

        yield return StartCoroutine(molarsHighlighter.HighlightTeeth());
        // molarsHighlighter.RemoveHighlight();
        Debug.Log("Molars highlight removed");
    }
}