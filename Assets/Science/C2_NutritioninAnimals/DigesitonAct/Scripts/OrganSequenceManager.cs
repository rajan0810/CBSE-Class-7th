using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OrganSequenceManager : MonoBehaviour
{
    public List<GameObject> organSequence;
    public List<GameObject> socketSequence;
    // public List<GameObject> originalTransformMarkers;

    private int currentOrganIndex = 0;
    private Dictionary<GameObject, bool> organPlacementStatus = new Dictionary<GameObject, bool>();

    public TextMeshProUGUI currentOrganText;

    private void Start()
    {
        // Initialize placement tracking
        foreach (GameObject organ in organSequence)
        {
            organPlacementStatus[organ] = false;  // All organs start as unplaced
        }

        if (organSequence.Count > 0)
        {
            UpdateCurrentOrganText();
        }
    }

    public GameObject GetCurrentOrgan()
    {
        while (currentOrganIndex < organSequence.Count && organPlacementStatus[organSequence[currentOrganIndex]])
        {
            currentOrganIndex++;
        }

        return (currentOrganIndex < organSequence.Count) ? organSequence[currentOrganIndex] : null;
    }

    public void OnOrganPlaced(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable, GameObject socket)
    {
        GameObject placedOrgan = interactable.gameObject;

        if (placedOrgan == GetCurrentOrgan())
        {
            organPlacementStatus[placedOrgan] = true; // Mark as placed

            // Find its listener & disable respawning
            OrganGrabListener grabListener = placedOrgan.GetComponent<OrganGrabListener>();
            if (grabListener != null)
            {
                grabListener.SetPlaced(true);
            }

            StartCoroutine(LockOrganInSocket(interactable, socket));

            if (currentOrganIndex < organSequence.Count - 1)
            {
                currentOrganIndex++;
                UpdateCurrentOrganText();
                Debug.Log("Next Organ: " + GetCurrentOrgan()?.name);
            }
            else
            {
                Debug.Log("All organs placed correctly!");
                currentOrganText.text = "All organs placed!";
            }
        }
    }

    private IEnumerator LockOrganInSocket(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable, GameObject socket)
    {
        yield return new WaitForSeconds(0.2f);

        interactable.transform.SetParent(socket.transform);
        interactable.enabled = false;

        Rigidbody rb = interactable.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Organ Locked in Place: " + interactable.gameObject.name);
    }

    private void UpdateCurrentOrganText()
    {
        if (currentOrganText != null)
        {
            GameObject nextOrgan = GetCurrentOrgan();
            currentOrganText.text = nextOrgan != null ? "Current Organ: " + nextOrgan.name : "All organs placed!";
        }
    }
}
