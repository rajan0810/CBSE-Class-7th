using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseLoader : MonoBehaviour
{
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;

    private GameObject leftHandInstance;

    private GameObject rightHandInstance;

    [TextArea(10, 30)] // Allows multiline input in the Unity Inspector
    public string handPoseData; // Input the formatted pose data directly in Inspector

    private Dictionary<string, Transform> leftHandJoints = new Dictionary<string, Transform>();
    private Dictionary<string, Transform> rightHandJoints = new Dictionary<string, Transform>();

    private Dictionary<string, Vector3> parsedHandPoseData = new Dictionary<string, Vector3>();

    void Start()
    {
        if (!string.IsNullOrEmpty(handPoseData))
        {
            parsedHandPoseData = ParsePoseData(handPoseData);
            LoadPose();
        }
        else
        {
            Debug.LogError("No pose data provided.");
        }
    }

    void LoadPose()
    {
        SpawnHands();
        ApplyHandPose(parsedHandPoseData);
    }

    void SpawnHands()
    {
        if (leftHandInstance != null) Destroy(leftHandInstance);
        if (rightHandInstance != null) Destroy(rightHandInstance);

        leftHandInstance = Instantiate(leftHandPrefab, Vector3.left * 0.3f, Quaternion.identity);
        rightHandInstance = Instantiate(rightHandPrefab, Vector3.right * 0.3f, Quaternion.identity);

        CollectHandJoints(leftHandInstance.transform, leftHandJoints);
        CollectHandJoints(rightHandInstance.transform, rightHandJoints);
    }

    void CollectHandJoints(Transform parent, Dictionary<string, Transform> jointDict)
    {
        foreach (Transform child in parent)
        {
            string jointName = child.name.Trim();
            if (!jointDict.ContainsKey(jointName))
            {
                jointDict[jointName] = child;
            }
            CollectHandJoints(child, jointDict);
        }
    }

    Dictionary<string, Vector3> ParsePoseData(string rawText)
    {
        Dictionary<string, Vector3> poseData = new Dictionary<string, Vector3>();

        string[] lines = rawText.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine)) continue;

            string[] parts = trimmedLine.Split('=');
            if (parts.Length != 2) continue;

            string jointName = parts[0].Trim(); // Example: "L_Wrist"
            string[] xyz = parts[1].Split(',');

            if (xyz.Length != 3) continue;

            if (float.TryParse(xyz[0], out float x) &&
                float.TryParse(xyz[1], out float y) &&
                float.TryParse(xyz[2], out float z))
            {
                poseData[jointName] = new Vector3(x, y, z); // Directly in degrees (Unity format)
            }
        }

        return poseData;
    }

    void ApplyHandPose(Dictionary<string, Vector3> poseData)
    {
        foreach (var joint in poseData)
        {
            string jointName = joint.Key;
            
            // ✅ Ignore Wrist Rotation
            if (jointName.EndsWith("_Wrist")) 
            {
                Debug.Log($"Ignoring wrist rotation for {jointName}");
                continue;
            }

            Vector3 rotationEuler = joint.Value; // Rotation already in degrees

            if (jointName.StartsWith("L_") && leftHandJoints.ContainsKey(jointName))
            {
                leftHandJoints[jointName].localRotation = Quaternion.Euler(rotationEuler);
            }
            else if (jointName.StartsWith("R_") && rightHandJoints.ContainsKey(jointName))
            {
                rightHandJoints[jointName].localRotation = Quaternion.Euler(rotationEuler);
            }
        }
    }
}
