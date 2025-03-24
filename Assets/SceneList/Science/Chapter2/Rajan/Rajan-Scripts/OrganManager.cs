using System.Collections;
using UnityEngine;

public class OrganManager : MonoBehaviour
{
    public GameObject[] organs; // Sequence of organs to highlight
    public float delayBetweenHighlights = 1f; // Delay before highlighting the next organ

    void Start()
    {
        StartCoroutine(HighlightSequence());
    }

    IEnumerator HighlightSequence()
    {
        foreach (GameObject organ in organs)
        {
            if (organ != null)
            {
                OrganHighlighter highlighter = organ.GetComponent<OrganHighlighter>();
                if (highlighter != null)
                {
                    yield return StartCoroutine(highlighter.HighlightOrgan()); // No argument needed now
                    yield return new WaitForSeconds(delayBetweenHighlights);
                }
            }
        }
    }
}
