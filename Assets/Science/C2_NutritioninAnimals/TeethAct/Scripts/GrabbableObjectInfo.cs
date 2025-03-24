using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GrabbableObjectInfo : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    // public TextMeshProUGUI tagText;

    void Awake()
    {
        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // Subscribe to grab event
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Print the object's tag when grabbed
        // tagText.text = "Tag: " + gameObject.tag;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}
