using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class HandAnimationSaver : MonoBehaviour
{
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;

    [TextArea(10, 30)]
    public string fistPoseData; // Rotation data for the fist

    void Start()
    {
        string animationFolder = "Assets/HandAnimations/";

        // ✅ Ensure the folder exists before saving
        if (!Directory.Exists(animationFolder))
        {
            Directory.CreateDirectory(animationFolder);
            AssetDatabase.Refresh();
        }

        CreateAndSaveAnimationClip(animationFolder + "NeutralToFist_L.anim", leftHandPrefab, fistPoseData, true);
        CreateAndSaveAnimationClip(animationFolder + "NeutralToFist_R.anim", rightHandPrefab, fistPoseData, false);
    }

    void CreateAndSaveAnimationClip(string savePath, GameObject handPrefab, string poseData, bool isLeftHand)
    {
        if (handPrefab == null || string.IsNullOrEmpty(poseData))
        {
            Debug.LogError("Hand prefab or pose data is missing!");
            return;
        }

        // Instantiate a temporary hand model to get its joints
        GameObject handInstance = Instantiate(handPrefab);
        Dictionary<string, Transform> handJoints = new Dictionary<string, Transform>();
        CollectHandJoints(handInstance.transform, handJoints);

        Dictionary<string, Vector3> targetRotations = ParsePoseData(poseData);

        AnimationClip animClip = new AnimationClip();
        animClip.legacy = false;

        // ✅ Sort joints by depth (Root → Child → Tip)
        List<KeyValuePair<string, Transform>> sortedJoints = handJoints
            .OrderBy(joint => GetHierarchyDepth(joint.Value)) // Sort based on depth
            .ToList();

        foreach (var joint in sortedJoints)
        {
            string jointName = joint.Key;

            if (isLeftHand && !jointName.StartsWith("L_")) continue;
            if (!isLeftHand && !jointName.StartsWith("R_")) continue;

            // ✅ Ignore the wrist rotation
            if (jointName.EndsWith("_Wrist")) continue;

            if (targetRotations.ContainsKey(jointName))
            {
                Transform bone = joint.Value;
                string path = GetBonePath(bone, handInstance.transform);

                // Convert Euler angles to Quaternions for smooth rotation
                Quaternion initialRotation = bone.localRotation;
                Quaternion targetRotation = Quaternion.Euler(targetRotations[jointName]);

                // Debugging: Log the joint transformation order
                Debug.Log($"Applying rotation to {jointName}: Depth={GetHierarchyDepth(bone)}, Initial={initialRotation.eulerAngles}, Target={targetRotations[jointName]}");

                // ✅ Apply animation with parent-first order
                AnimationCurve curveX = AnimationCurve.Linear(0f, initialRotation.eulerAngles.x, 1f, targetRotation.eulerAngles.x);
                AnimationCurve curveY = AnimationCurve.Linear(0f, initialRotation.eulerAngles.y, 1f, targetRotation.eulerAngles.y);
                AnimationCurve curveZ = AnimationCurve.Linear(0f, initialRotation.eulerAngles.z, 1f, targetRotation.eulerAngles.z);

                animClip.SetCurve(path, typeof(Transform), "localEulerAngles.x", curveX);
                animClip.SetCurve(path, typeof(Transform), "localEulerAngles.y", curveY);
                animClip.SetCurve(path, typeof(Transform), "localEulerAngles.z", curveZ);
            }
        }

        // ✅ Ensure the folder exists before saving the animation clip
        string directoryPath = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            AssetDatabase.Refresh();
        }

        // ✅ Save the animation clip
        AssetDatabase.CreateAsset(animClip, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Animation saved: {savePath}");

        // Cleanup
        Destroy(handInstance);
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
            string[] parts = line.Split('=');
            if (parts.Length != 2) continue;

            string jointName = parts[0].Trim();
            string[] xyz = parts[1].Split(',');

            if (xyz.Length != 3) continue;

            if (float.TryParse(xyz[0], out float x) &&
                float.TryParse(xyz[1], out float y) &&
                float.TryParse(xyz[2], out float z))
            {
                poseData[jointName] = new Vector3(x, y, z);
            }
        }

        return poseData;
    }

    string GetBonePath(Transform bone, Transform root)
    {
        string path = bone.name;
        while (bone.parent != null && bone.parent != root)
        {
            bone = bone.parent;
            path = bone.name + "/" + path;
        }
        return path;
    }

    int GetHierarchyDepth(Transform bone)
    {
        int depth = 0;
        while (bone.parent != null)
        {
            depth++;
            bone = bone.parent;
        }
        return depth;
    }
}
