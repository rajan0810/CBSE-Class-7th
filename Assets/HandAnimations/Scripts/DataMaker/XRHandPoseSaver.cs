using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Hands;

public class XRHandPoseSaver : MonoBehaviour
{
    public XRHandLogger xRHandLogger;
    private XRHandSubsystem handSubsystem;

    [Header("UI Elements")]
    public Text countdownText;  // UI for status messages
    public Text poseDataText;   // UI to show the saved pose data

    public float captureDelay = 5f; // Time before recording starts

    void Start()
    {
        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        foreach (var subsystem in handSubsystems)
        {
            if (subsystem.running)
            {
                handSubsystem = subsystem;
                UpdateCountdownUI(" XR Hand Tracking is ACTIVE!");
                break;
            }
        }

        if (handSubsystem != null)
        {
            handSubsystem.updatedHands += OnUpdatedHands;
        }
        else
        {
            UpdateCountdownUI(" Error: No active XRHandSubsystem found!");
        }
    }

    void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        // Not recording continuously
    }

    public void StartPoseCapture()
    {
        StartCoroutine(CapturePoseRoutine());
    }

    private IEnumerator CapturePoseRoutine()
    {
        // Countdown before recording starts
        for (int i = (int)captureDelay; i > 0; i--)
        {
            UpdateCountdownUI($" Get ready! Capturing pose in {i} seconds...");
            yield return new WaitForSeconds(1);
        }

        UpdateCountdownUI(" Capturing pose...");

        // Capture a single frame of hand data
        Dictionary<string, Dictionary<string, Vector3>> poseData = CaptureHandPose();

        if (poseData.Count == 0)
        {
            UpdateCountdownUI(" No hand data captured! Make sure hands are tracked.");
            yield break;  // Stop execution if no data is captured
        }

        // Convert to human-readable text format
        string formattedData = FormatPoseDataAsText(poseData);

        // Save the pose data as a .txt file'
        string data = xRHandLogger.SaveData();
        DisplaySavedText(data);
        // bool success = SavePoseData(formattedData);

        // if (success)
        // {
        //     UpdateCountdownUI(" Pose saved successfully!");
        //     DisplaySavedText(formattedData); // Show exactly what was saved
        // }
        // else
        // {
        //     UpdateCountdownUI(" Error: Could not save pose!");
        // }
    }

    private void UpdateCountdownUI(string message)
    {
        if (countdownText != null)
        {
            countdownText.text = message;
        }
    }

    private void DisplaySavedText(string textData)
    {
        if (poseDataText != null)
        {
            poseDataText.text = textData;
        }
    }

    private Dictionary<string, Dictionary<string, Vector3>> CaptureHandPose()
    {
        Dictionary<string, Dictionary<string, Vector3>> handPoseData = new Dictionary<string, Dictionary<string, Vector3>>();

        if (handSubsystem == null)
        {
            UpdateCountdownUI(" Error: XRHandSubsystem is not initialized.");
            return handPoseData;
        }

        if (handSubsystem.leftHand.isTracked)
        {
            handPoseData["LeftHand"] = CaptureHandData(handSubsystem.leftHand);
        }
        else
        {
            UpdateCountdownUI("⚠️ Left hand is NOT being tracked.");
        }

        if (handSubsystem.rightHand.isTracked)
        {
            handPoseData["RightHand"] = CaptureHandData(handSubsystem.rightHand);
        }
        else
        {
            UpdateCountdownUI("⚠️ Right hand is NOT being tracked.");
        }

        return handPoseData;
    }

    private Dictionary<string, Vector3> CaptureHandData(XRHand hand)
    {
        Dictionary<string, Vector3> jointRotations = new Dictionary<string, Vector3>();

        foreach (XRHandJointID jointID in (XRHandJointID[])System.Enum.GetValues(typeof(XRHandJointID)))
        {
            var joint = hand.GetJoint(jointID);

            if (joint.TryGetPose(out Pose pose))
            {
                Quaternion worldRotation = pose.rotation; // Corrected: Direct world-space rotation
                Vector3 finalRotation = worldRotation.eulerAngles; // Convert to Euler angles
                jointRotations[jointID.ToString()] = finalRotation;
            }
        }

        return jointRotations;
    }

    private string FormatPoseDataAsText(Dictionary<string, Dictionary<string, Vector3>> data)
    {
        string textOutput = "Captured Hand Pose Data:\n\n";

        foreach (var hand in data)
        {
            textOutput += $"{hand.Key}:\n";

            foreach (var joint in hand.Value)
            {
                Vector3 rotation = joint.Value;
                textOutput += $"{joint.Key}: X={rotation.x:F2}, Y={rotation.y:F2}, Z={rotation.z:F2}\n";
            }

            textOutput += "\n"; // Separate left and right hands
        }

        return textOutput;
    }

    private bool SavePoseData(string textData)
    {
        try
        {
            if (string.IsNullOrEmpty(textData))
            {
                UpdateCountdownUI(" Error: No data to save!");
                return false;
            }

            string path = "/storage/emulated/0/Documents/hand_pose.txt"; // Save as .txt

            File.WriteAllText(path, textData);
            return true;
        }
        catch (System.Exception e)
        {
            UpdateCountdownUI($" Error: {e.Message}");
            return false;
        }
    }
}
