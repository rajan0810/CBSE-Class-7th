using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class OrganGrabListener : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;
    private OrganSequenceManager sequenceManager;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isPlaced = false;  // 🔥 Tracks if this organ is placed

    private void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (interactable == null)
        {
            Debug.LogError("OrganGrabListener requires an XRGrabInteractable component.");
            return;
        }

        // Store the initial position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Find the OrganSequenceManager in the scene
        sequenceManager = FindFirstObjectByType<OrganSequenceManager>();

        // Listen for grab and drop events
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (sequenceManager == null)
        {
            Debug.LogError("OrganSequenceManager not found.");
            return;
        }

        // 🚨 If the organ is already placed, do nothing!
        if (isPlaced)
        {
            Debug.Log("Organ already placed: " + gameObject.name);
            return;
        }

        // If incorrect, release & respawn it
        if (sequenceManager.GetCurrentOrgan() != gameObject)
        {
            StartCoroutine(ReleaseAndRespawn(args.interactorObject));
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (isPlaced)
        {
            return; // 🚨 If already placed, do nothing!
        }

        // If the correct organ is dropped but NOT placed, respawn it
        if (sequenceManager.GetCurrentOrgan() == gameObject)
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Small delay before respawning

        // 🚨 Ensure it hasn't been placed in the meantime
        if (isPlaced) yield break;

        Debug.Log("Organ dropped! Respawning: " + gameObject.name);

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private IEnumerator ReleaseAndRespawn(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactorObject)
    {
        yield return new WaitForSeconds(1f);

        // If organ is already placed, do not respawn!
        if (isPlaced) yield break;

        if (interactorObject != null && interactable != null && interactable.interactionManager != null)
        {
            interactable.interactionManager.SelectExit(interactorObject, interactable);
        }

        yield return new WaitForSeconds(0.1f);

        // Reset position & rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Incorrect organ released and respawned: " + gameObject.name);
    }

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
            interactable.selectExited.RemoveListener(OnSelectExited);
        }
    }
}
