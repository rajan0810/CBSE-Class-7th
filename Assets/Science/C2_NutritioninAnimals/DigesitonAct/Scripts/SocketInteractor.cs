using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketInteractor : MonoBehaviour
{
    private OrganSequenceManager sequenceManager;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        sequenceManager = FindFirstObjectByType<OrganSequenceManager>();

        if (sequenceManager == null)
        {
            Debug.LogError("OrganSequenceManager not found in the scene!");
        }

        if (socketInteractor == null)
        {
            Debug.LogError("XRSocketInteractor component is missing on " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnOrganIsPlaced);
        }
    }

    private void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnOrganIsPlaced);
        }
    }

    private void OnOrganIsPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject == null)
        {
            Debug.LogWarning("Placed object is null.");
            return;
        }

        GameObject placedObject = args.interactableObject.transform.gameObject;
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable = placedObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (interactable != null)
        {
            sequenceManager.OnOrganPlaced(interactable, gameObject);
        }
        else
        {
            Debug.LogError("XRGrabInteractable not found on placed object.");
        }
    }
}
