using UnityEngine;

public class HandAnimationController : MonoBehaviour
{
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    void Update()
    {
        // Update animations for both hands
        UpdateHandAnimation(leftHandAnimator);
        UpdateHandAnimation(rightHandAnimator);
    }

    void UpdateHandAnimation(Animator handAnimator)
    {
        bool isGripPressed = Input.GetButton("Grip");  // Replace with VR input later
        bool isTriggerPressed = Input.GetButton("Trigger");

        int handState = 0; // Default state: Normal Hand

        if (isGripPressed && isTriggerPressed)
        {
            handState = 3; // Fist (Both Grip + Trigger)
        }
        else if (isTriggerPressed)
        {
            handState = 2; // Point (Only Trigger)
        }
        else if (isTriggerPressed && !isGripPressed)
        {
            handState = 1; // Pinch (Light Trigger Press)
        }

        handAnimator.SetInteger("HandState", handState);
    }
}
