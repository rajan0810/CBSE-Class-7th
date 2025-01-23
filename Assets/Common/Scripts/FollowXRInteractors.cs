using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
[System.Serializable]
public struct SideDefinition {
    public Transform HandRoot;
    public Transform ControllerRoot;
}
public enum Track { 
       RIGHT,LEFT,HEAD
}
public class FollowXRInteractors : MonoBehaviour
{

    SideDefinition right;
    SideDefinition left;
    Transform head;

    public Vector3 offset;
    public float trackingSharpness;
    public float deadZone;

    public Track target;

    private Transform leader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        right = PlayerManager.instance.right;
        left = PlayerManager.instance.left;
        head = PlayerManager.instance.head;
        updateTracking();
    }
    public void updateTracking()
    {
        switch (target)
        {
            case Track.LEFT:
                if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.TrackedHand)
                {
                    leader = left.HandRoot;
                }
                else if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.MotionController)
                {
                    leader = left.ControllerRoot;
                }
                break;
            case Track.RIGHT:
                if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.TrackedHand)
                {
                    leader = right.HandRoot;
                }
                else if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.MotionController)
                {
                    leader = right.ControllerRoot;
                }
                break;
            case Track.HEAD:
                leader = head;
                break;
            default:
                Debug.Log("???");
                break;
        }
    }
    public void updateTracking(Track mode) {
        target = mode;
        switch (mode) { 
            case Track.LEFT:
                if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.TrackedHand)
                {
                    leader = left.HandRoot;
                }
                else if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.MotionController) {
                    leader = left.ControllerRoot;
                }
                break;
            case Track.RIGHT:
                if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.TrackedHand)
                {
                    leader = right.HandRoot;
                }
                else if (XRInputModalityManager.currentInputMode.Value == XRInputModalityManager.InputMode.MotionController)
                {
                    leader = right.ControllerRoot;
                }
                break;
            case Track.HEAD:
                leader = head;
                break;
            default:
                Debug.Log("???");
                break;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position += ((leader.position + offset) - transform.position) * trackingSharpness;
    }
}
