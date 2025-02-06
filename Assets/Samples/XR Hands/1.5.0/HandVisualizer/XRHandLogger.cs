using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.UI;
using UnityEngine.InputSystem; // New Input System

public class XRHandLogger : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    public GameObject leftHand; // Assign the left-hand model in the Inspector
    public Text logText; // Assign a UI Text element in the Inspector

    private Dictionary<XRHandJointID, Transform> handJoints = new Dictionary<XRHandJointID, Transform>(); // Store joints by ID

    void Start()
    {
        if (leftHand != null)
        {
            CollectHandJoints(leftHand.transform);
        }

        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        foreach (var subsystem in handSubsystems)
        {
            if (subsystem.running)
            {
                handSubsystem = subsystem;
                break;
            }
        }

        if (handSubsystem != null)
        {
            handSubsystem.updatedHands += OnUpdatedHands;
        }
        else
        {
            Debug.LogError("No active XRHandSubsystem found!");
            UpdateUIText("No active XRHandSubsystem found!");
        }
    }

    // Recursively collect all joints into handJoints dictionary
    void CollectHandJoints(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Match child names to known joint names
            foreach (XRHandJointID jointID in (XRHandJointID[])System.Enum.GetValues(typeof(XRHandJointID)))
            {
                if (child.name.ToLower().Contains(jointID.ToString().ToLower()))
                {
                    handJoints[jointID] = child;
                    break;
                }
            }
            CollectHandJoints(child); // Recursively process children
        }
    }

    void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose) != 0)
        {
            string logData = MapLeftHandRotations(handSubsystem.leftHand);
            UpdateUIText(logData);
        }
    }

    string MapLeftHandRotations(XRHand hand)
    {
        if (!hand.isTracked)
        {
            return "Left Hand is not being tracked.\n";
        }

        string logMessage = "Left Hand Rotations:\n";

        foreach (var jointID in handJoints.Keys)
        {
            var joint = hand.GetJoint(jointID);

            if (joint.TryGetPose(out Pose pose))
            {
                Transform jointTransform = handJoints[jointID];

                if (jointTransform != null)
                {
                    // Convert world-space rotation to local-space
                    jointTransform.localRotation = Quaternion.Inverse(jointTransform.parent.rotation) * pose.rotation;
                }

                // Append rotation info to UI text
                logMessage += $"{jointID}: {pose.rotation.eulerAngles}\n";
            }
        }

        return logMessage;
    }

    void UpdateUIText(string message)
    {
        if (logText != null)
        {
            logText.text = message;
        }
    }

    void OnDestroy()
    {
        if (handSubsystem != null)
        {
            handSubsystem.updatedHands -= OnUpdatedHands;
        }
    }
}
