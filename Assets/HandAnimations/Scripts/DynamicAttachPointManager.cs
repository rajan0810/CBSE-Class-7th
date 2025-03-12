using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DynamicAttachPointManager : MonoBehaviour
{
    [System.Serializable]
    public class AttachPointMapping
    {
        public string objectIdentifier; // Name, tag, or layer
        public Transform attachPoint;
        [Tooltip("How to identify the object (by Name, Tag, or Layer)")]
        public IdentificationType identificationType = IdentificationType.Name;
    }

    public enum IdentificationType
    {
        Name,
        Tag,
        Layer
    }

    [SerializeField]
    private List<AttachPointMapping> attachPointMappings = new List<AttachPointMapping>();

    [SerializeField]
    private Transform defaultAttachPoint;

    [SerializeField]
    private InteractionAttachController attachController;

    private Transform originalAttachPoint;

    private void Awake()
    {
        if (attachController == null)
            attachController = GetComponent<InteractionAttachController>();

        if (attachController != null)
            originalAttachPoint = attachController.transformToFollow;
        else
            Debug.LogError("No InteractionAttachController found!", this);
    }

    private void OnEnable()
    {
        // Subscribe to selection events
        var interactor = GetComponent<XRBaseInteractor>();
        if (interactor != null)
        {
            interactor.selectEntered.AddListener(OnSelectEntered);
            interactor.selectExited.AddListener(OnSelectExited);
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from selection events
        var interactor = GetComponent<XRBaseInteractor>();
        if (interactor != null)
        {
            interactor.selectEntered.RemoveListener(OnSelectEntered);
            interactor.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (attachController == null || args.interactableObject == null)
            return;

        GameObject grabbedObject = args.interactableObject.transform.gameObject;
        Transform matchingAttachPoint = FindMatchingAttachPoint(grabbedObject);
        
        if (matchingAttachPoint != null)
        {
            // Change the transform to follow
            attachController.transformToFollow = matchingAttachPoint;
            Debug.Log($"Changed attach point for {grabbedObject.name} to {matchingAttachPoint.name}");
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (attachController == null)
            return;

        // Reset to original attach point
        attachController.transformToFollow = originalAttachPoint;
    }

    private Transform FindMatchingAttachPoint(GameObject interactableObject)
    {
        foreach (var mapping in attachPointMappings)
        {
            bool isMatch = false;
            
            switch (mapping.identificationType)
            {
                case IdentificationType.Name:
                    isMatch = interactableObject.name.Contains(mapping.objectIdentifier);
                    break;
                case IdentificationType.Tag:
                    isMatch = interactableObject.CompareTag(mapping.objectIdentifier);
                    break;
                case IdentificationType.Layer:
                    isMatch = interactableObject.layer == LayerMask.NameToLayer(mapping.objectIdentifier);
                    break;
            }
            
            if (isMatch && mapping.attachPoint != null)
                return mapping.attachPoint;
        }
        
        // Return default if no match found
        return defaultAttachPoint != null ? defaultAttachPoint : originalAttachPoint;
    }

    // Editor utility method to add a new mapping
    public void AddMapping(string identifier, Transform attachPoint, IdentificationType type = IdentificationType.Name)
    {
        attachPointMappings.Add(new AttachPointMapping
        {
            objectIdentifier = identifier,
            attachPoint = attachPoint,
            identificationType = type
        });
    }
}
