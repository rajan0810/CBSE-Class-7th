using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.IO; 

public class XRHandLogger : MonoBehaviour
{
    public HeadsetWebSocketClient headsetWebSocketClient;
    private XRHandSubsystem handSubsystem;
    public GameObject leftHand;  // Assign the left-hand model in the Inspector
    public GameObject rightHand; // Assign the right-hand model in the Inspector
    public Text logText;         // Assign a UI Text element in the Inspector

    private Dictionary<XRHandJointID, Transform> leftHandJoints = new Dictionary<XRHandJointID, Transform>(); // Store left-hand joints
    private Dictionary<XRHandJointID, Transform> rightHandJoints = new Dictionary<XRHandJointID, Transform>(); // Store right-hand joints

    void Start()
    {
        if (leftHand != null)
        {
            CollectHandJoints(leftHand.transform, leftHandJoints);
        }

        if (rightHand != null)
        {
            CollectHandJoints(rightHand.transform, rightHandJoints);
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

    // Recursively collect all joints into the appropriate dictionary
    void CollectHandJoints(Transform parent, Dictionary<XRHandJointID, Transform> handJoints)
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
            CollectHandJoints(child, handJoints); // Recursively process children
        }
    }

    void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        string logData = "";

        if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose) != 0)
        {
            logData += MapHandRotations(handSubsystem.leftHand, leftHandJoints, "Left Hand");
        }

        if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose) != 0)
        {
            logData += MapHandRotations(handSubsystem.rightHand, rightHandJoints, "Right Hand");
        }

        UpdateUIText(logData);
    }

    string MapHandRotations(XRHand hand, Dictionary<XRHandJointID, Transform> handJoints, string handName)
    {
        if (!hand.isTracked)
        {
            return $"{handName} is not being tracked.\n";
        }

        string logMessage = ""; //= $"{handName} Rotations:\n";

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
                    
                    // Get final rotation in Euler angles (X, Y, Z)
                    Vector3 finalRotation = jointTransform.localRotation.eulerAngles;

                    if(handName == "Left Hand")
                    {
                        logMessage += $"L_{jointID}={finalRotation.x:F2},{finalRotation.y:F2},{finalRotation.z:F2}\n";

                    }
                    else if(handName == "Right Hand")
                    {
                        logMessage += $"R_{jointID}={finalRotation.x:F2},{finalRotation.y:F2},{finalRotation.z:F2}\n";
                    }
                    // Append rotation info to UI text
                    
                }
            }
        }

        return logMessage;
    }



    public string SaveData()
    {
        string response;
    
        if (logText == null)
        {
            response = "UI Text is not assigned! Please assign a Text UI element in the Inspector.";
            Debug.LogError("UI Text is not assigned! Please assign a Text UI element in the Inspector.");

            return response;
        }

        string currentText = logText.text; // Get the current UI text
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"); // Generate timestamp
        string fileName = $"UIText_{timestamp}.txt"; // File name with timestamp

        headsetWebSocketClient.SendData(currentText);

        // Use Application.persistentDataPath (Cross-platform)
        string path  = "/storage/emulated/0/Documents/hand_pose.txt";
        // Save text to file
        File.WriteAllText(path, currentText);
        response = $"UI text saved to: {path}";
        return response;
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
