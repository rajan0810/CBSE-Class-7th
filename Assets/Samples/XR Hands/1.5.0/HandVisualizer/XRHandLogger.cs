using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.UI;
using System.IO; // Required for file handling

public class XRHandMapper : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    public Text logText; // Assign a UI Text element in the Inspector
    public GameObject leftHand; // Assign the left-hand model in the Inspector

    private List<Transform> l_handTransforms = new List<Transform>(); // Store all hand joints
    private string filePath; // File path for logging data

    void Start()
    {
        if (leftHand != null)
        {
            CollectChildTransforms(leftHand.transform);
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

        // Define file path to store the logs
        filePath = Application.persistentDataPath + "/LeftHandJointLog.txt";
    }

    // Recursively collect all children into l_handTransforms list
    void CollectChildTransforms(Transform parent)
    {
        foreach (Transform child in parent)
        {
            l_handTransforms.Add(child);
            CollectChildTransforms(child); // Recursively process sub-children
        }
    }

    void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose) != 0)
        {
            string logData = MapLeftHand(handSubsystem.leftHand);
            UpdateUIText(logData);
        }
    }

    string MapLeftHand(XRHand hand)
    {
        if (!hand.isTracked)
        {
            return "Left Hand is not being tracked.\n";
        }

        string logMessage = $"Left Hand Root Position: {hand.rootPose.position}, Rotation: {hand.rootPose.rotation}\n";

        int jointIndex = 0;

        for (var i = XRHandJointID.BeginMarker.ToIndex(); i < XRHandJointID.EndMarker.ToIndex(); i++)
        {
            var jointID = XRHandJointIDUtility.FromIndex(i);
            var joint = hand.GetJoint(jointID);

            if (joint.TryGetPose(out Pose pose) && jointIndex < l_handTransforms.Count)
            {
                Transform jointTransform = l_handTransforms[jointIndex];

                if (jointTransform != null)
                {
                    // Convert world-space rotation to local-space
                    jointTransform.localRotation = Quaternion.Inverse(jointTransform.parent.rotation) * pose.rotation;
                    
                    // Convert world-space position to local-space
                    jointTransform.localPosition = jointTransform.parent.InverseTransformPoint(pose.position);
                }

                logMessage += $"Joint {jointID}: Position: {pose.position}, Rotation: {pose.rotation}\n";
            }

            jointIndex++;
        }

        return logMessage;
    }

    void Update()
    {
        // Log data when Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LogHandDataToFile();
        }
    }

    void LogHandDataToFile()
    {
        if (handSubsystem == null || !handSubsystem.leftHand.isTracked)
        {
            Debug.LogWarning("Left hand is not tracked. Cannot log data.");
            return;
        }

        string logData = $"[Time: {System.DateTime.Now}]\n";

        int jointIndex = 0;

        for (var i = XRHandJointID.BeginMarker.ToIndex(); i < XRHandJointID.EndMarker.ToIndex(); i++)
        {
            var jointID = XRHandJointIDUtility.FromIndex(i);
            var joint = handSubsystem.leftHand.GetJoint(jointID);

            if (joint.TryGetPose(out Pose pose) && jointIndex < l_handTransforms.Count)
            {
                logData += $"Joint {jointID}:\n Position: {pose.position}\n Rotation: {pose.rotation}\n";
            }

            jointIndex++;
        }

        // Write to file
        File.AppendAllText(filePath, logData + "\n----------------------\n");

        Debug.Log($"Logged Left Hand Data to: {filePath}");
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
