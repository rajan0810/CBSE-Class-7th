using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimationController : MonoBehaviour
{
    public Text logText;    
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    // [Header("Input Actions")]
    // public InputActionReference leftGrip;
    // public InputActionReference rightGrip;
    // public InputActionReference leftTrigger;
    // public InputActionReference rightTrigger;
    //
    // private float leftGripValue = 0f;
    // private float rightGripValue = 0f;
    // private float leftTriggerValue = 0f;
    // private float rightTriggerValue = 0f;
    //
    
    [Header("XR Interactors")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor leftNearFarInteractor;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor rightNearFarInteractor;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabbedObjectLeft;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabbedObjectRight;

    void Start()
    {
        // // // Register input action events
        // leftGrip.action.performed += ctx => leftGripValue = ctx.ReadValue<float>();
        // rightGrip.action.performed += ctx => rightGripValue = ctx.ReadValue<float>();
        // leftTrigger.action.performed += ctx => leftTriggerValue = ctx.ReadValue<float>();
        // rightTrigger.action.performed += ctx => rightTriggerValue = ctx.ReadValue<float>();
        //
        // // //Reset the vlaue on relesed
        // leftGrip.action.canceled += ctx => leftGripValue = 0f;
        // rightGrip.action.canceled += ctx => rightGripValue = 0f;
        // leftTrigger.action.canceled += ctx => leftTriggerValue = 0f;
        // rightTrigger.action.canceled += ctx => rightTriggerValue = 0f;
        //
        // Subscribe to grab and release events
        leftNearFarInteractor.selectEntered.AddListener(OnLeftGrab);
        leftNearFarInteractor.selectExited.AddListener(OnLeftRelease);
        rightNearFarInteractor.selectEntered.AddListener(OnRightGrab);
        rightNearFarInteractor.selectExited.AddListener(OnRightRelease);
    }

    void OnLeftGrab(SelectEnterEventArgs args)
    {
        if (args != null)
        {
            GameObject interactableGameObject = args.interactableObject as MonoBehaviour != null ? (args.interactableObject as MonoBehaviour).gameObject : null;
            if (interactableGameObject != null)
            {
                string triggnerName = interactableGameObject.tag;
                triggnerName += "Play";
                UpdateUIText(triggnerName);
                leftHandAnimator.SetTrigger(triggnerName);
            }
        }
    }

    void OnLeftRelease(SelectExitEventArgs args)
    {
        if (args != null)
        {
            GameObject interactableGameObject = args.interactableObject as MonoBehaviour != null ? (args.interactableObject as MonoBehaviour).gameObject : null;
            if (interactableGameObject != null)
            {
                string triggnerName = interactableGameObject.tag;
                triggnerName += "Exit";
                UpdateUIText(triggnerName);
                leftHandAnimator.SetTrigger(triggnerName);
            }
        }
    }
    
    void OnRightGrab(SelectEnterEventArgs args)
    {
       
        if (args != null)
        {
            GameObject interactableGameObject = args.interactableObject as MonoBehaviour != null ? (args.interactableObject as MonoBehaviour).gameObject : null;
            if (interactableGameObject != null)
            {
                string triggnerName = interactableGameObject.tag;
                triggnerName += "Play";
                UpdateUIText(triggnerName);
                rightHandAnimator.SetTrigger(triggnerName);
            }
        }
    }

    void OnRightRelease(SelectExitEventArgs args)
    {
        if (args != null)
        {
            GameObject interactableGameObject = args.interactableObject as MonoBehaviour != null ? (args.interactableObject as MonoBehaviour).gameObject : null;
            if (interactableGameObject != null)
            {
                string triggnerName = interactableGameObject.tag;
                triggnerName += "Exit";
                UpdateUIText(triggnerName);
                rightHandAnimator.SetTrigger(triggnerName);
            }
        }
    }
    
    void Update()
    {
        // Update hand animations (both for button input & grabbing objects)
        //UpdateHandAnimation(leftHandAnimator, leftGripPressed, leftTriggerPressed, grabbedObjectLeft);
        //UpdateHandAnimation(rightHandAnimator, rightGripPressed, rightTriggerPressed, grabbedObjectRight);

        // Update UI Debug Info
        //UpdateUIText($"Left Hand: Grip={leftGripPressed}, Trigger={leftTriggerPressed}, Holding={(grabbedObjectLeft != null ? grabbedObjectLeft.name : "None")}\n" +
                    // $"Right Hand: Grip={rightGripPressed}, Trigger={rightTriggerPressed}, Holding={(grabbedObjectRight != null ? grabbedObjectRight.name : "None")}");
        //UpdateUIText($"LT: {leftTriggerValue},LG: {leftGripValue} \n"+$"RT:{rightTriggerValue},RG:{rightGripValue}");
    }

    // void UpdateHandAnimation(Animator handAnimator, bool isGripPressed, bool isTriggerPressed, UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbedObject)
    // {
    //     int handState = 0; // Default: Normal Hand
    //
    //     if (grabbedObject) 
    //     {
    //         handState = 3; // Grab Animation
    //     }
    //     else if (isGripPressed && isTriggerPressed)
    //     {
    //         handState = 3; // Fist (Both Grip + Trigger)
    //     }
    //     else if (isTriggerPressed)
    //     {
    //         handState = 2; // Point (Only Trigger)
    //     }
    //     else if (!isGripPressed && isTriggerPressed)
    //     {
    //         handState = 1; // Pinch (Light Trigger Press)
    //     }
    //
    //     handAnimator.SetInteger("HandState", handState);
    // }
    //
    void UpdateUIText(string message)
    {
        if (logText)
        {
            logText.text = message;
        }
    }
}