using UnityEngine;

public class SpriteSheetSwitcherOnStart : MonoBehaviour
{
    [Tooltip("Reference to the SpriteSheetSwitcher component (auto-assigned if left empty).")]
    public FaceAnimationController faceAnimationController;

    private void Start()
    {
        if (faceAnimationController == null)
        {
            faceAnimationController = GetComponent<FaceAnimationController>();
        }
        
        if (faceAnimationController == null)
        {
            Debug.LogError("SpriteSheetSwitcherOnStart: SpriteSheetSwitcher component not found!");
            return;
        }
        
        // Automatically start the infinite animation.
        faceAnimationController.PlayIdleAnimation();
    }
}