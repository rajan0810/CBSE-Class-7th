using UnityEngine;

public class SpriteSheetSwitcherOnStart : MonoBehaviour
{
    [Tooltip("Reference to the SpriteSheetSwitcher component (auto-assigned if left empty).")]
    public SpriteSheetSwitcher spriteSheetSwitcher;

    private void Start()
    {
        if (spriteSheetSwitcher == null)
        {
            spriteSheetSwitcher = GetComponent<SpriteSheetSwitcher>();
        }
        
        if (spriteSheetSwitcher == null)
        {
            Debug.LogError("SpriteSheetSwitcherOnStart: SpriteSheetSwitcher component not found!");
            return;
        }
        
        // Automatically start the infinite animation.
        spriteSheetSwitcher.CycleSpriteAnimation();
    }
}