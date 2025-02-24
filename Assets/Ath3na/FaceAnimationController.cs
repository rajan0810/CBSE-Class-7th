using UnityEngine;
using System.Collections;

public class FaceAnimationController : MonoBehaviour
{
    [Tooltip("The material using the unlit sprite-sheet shader.")]
    public Material targetMaterial;
    
    [Tooltip("Default Y offset for the first sprite (sprite index 0).")]
    public float defaultOffset = 0f;
    
    public Texture[] spriteSheets;

    [Tooltip("UV offset increment for each sprite (for 7 sprites, e.g., 1/7).")]
    public float spriteIncrement = 1f / 7f;

    [Tooltip("Time interval (in seconds) between each sprite switch.")]
    public float cycleInterval = 0.2f;
    
    [Tooltip("Total number of sprites on the sheet (including the default sprite).")]
    public int totalSprites = 7;

    // Precomputed sequence for a smooth ping-pong cycle.
    // For totalSprites = 7, the sequence will be: 0, 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1.
    private int[] sequence;
    // Current index in the sequence.
    private int sequenceIndex = 0;
    
    // Animation flag.
    private bool isAnimating = false;
    public void SwitchSpriteSheet(int index)
    {
        if (spriteSheets == null || index < 0 || index >= spriteSheets.Length)
        {
            Debug.LogError("SpriteSheetSwitcherWithTextureSwitch: Sprite sheet index out of range.");
            return;
        }
        // Change the material's base map to the new sprite sheet.
        targetMaterial.mainTexture = spriteSheets[index];
        // Reset animation state.
        ResetSprite();
        // Optionally, start the animation.
        CycleSpriteAnimation();
    }
    
    public void PlayIdleAnimation()
    {
        // Assuming idle texture is at index 0.
        SwitchSpriteSheet(0);
    }
    
    public void PlayHappyAnimation()
    {
        // Assuming idle texture is at index 0.
        SwitchSpriteSheet(1);
    }

    private void Start()
    {
        // Check that the target material is assigned.
        if (targetMaterial == null)
        {
            Debug.LogError("SpriteSheetSwitcher: Target Material is not assigned!");
            return;
        }
        
        // Build the sequence.
        BuildSequence();
        
        // Initialize the material to the default sprite.
        if (sequence != null && sequence.Length > 0)
        {
            SetSprite(sequence[sequenceIndex]);
        }
        else
        {
            Debug.LogError("SpriteSheetSwitcher: Sequence was not built properly.");
        }
    }

    /// <summary>
    /// Call this function (via UI, another script, etc.) to start the infinite ping-pong animation.
    /// </summary>
    public void CycleSpriteAnimation()
    {
        // If targetMaterial is null, log an error and exit.
        if (targetMaterial == null)
        {
            Debug.LogError("SpriteSheetSwitcher: Target Material is not assigned, cannot animate sprite sheet.");
            return;
        }
        // If the sequence hasn't been built, build it now.
        if (sequence == null || sequence.Length == 0)
        {
            BuildSequence();
        }
        
        if (!isAnimating)
        {
            StartCoroutine(AnimateCycle());
        }
    }

    /// <summary>
    /// Precomputes a ping-pong sequence without duplicating the endpoints.
    /// For totalSprites = 7, the sequence will be:
    /// 0, 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1.
    /// </summary>
    private void BuildSequence()
    {
        if (totalSprites < 2)
        {
            Debug.LogError("SpriteSheetSwitcher: totalSprites must be at least 2.");
            return;
        }
        
        // Forward: totalSprites values; Reverse: totalSprites - 2 values.
        int sequenceLength = totalSprites + (totalSprites - 2);
        sequence = new int[sequenceLength];
        int idx = 0;
        
        // Forward: 0 to totalSprites - 1.
        for (int i = 0; i < totalSprites; i++)
        {
            sequence[idx++] = i;
        }
        
        // Reverse: from totalSprites - 2 down to 1.
        for (int i = totalSprites - 2; i >= 1; i--)
        {
            sequence[idx++] = i;
        }
    }

    /// <summary>
    /// Coroutine that continuously iterates through the precomputed sequence.
    /// </summary>
    private IEnumerator AnimateCycle()
    {
        isAnimating = true;
        while (true)
        {
            // Advance to the next sprite in the sequence.
            sequenceIndex = (sequenceIndex + 1) % sequence.Length;
            SetSprite(sequence[sequenceIndex]);
            yield return new WaitForSeconds(cycleInterval);
        }
    }

    /// <summary>
    /// Updates the material's Y offset based on the given sprite index.
    /// </summary>
    /// <param name="spriteIndex">Index of the sprite (0 to totalSprites - 1).</param>
    private void SetSprite(int spriteIndex)
    {
        if (targetMaterial != null)
        {
            // Calculate the new Y offset.
            float newYOffset = defaultOffset + spriteIndex * spriteIncrement;
            Vector2 currentOffset = targetMaterial.mainTextureOffset;
            targetMaterial.mainTextureOffset = new Vector2(currentOffset.x, newYOffset);
        }
        else
        {
            Debug.LogError("SpriteSheetSwitcher: targetMaterial is null in SetSprite.");
        }
    }

    /// <summary>
    /// (Optional) Resets the sprite to the default (index 0) and stops any running animation.
    /// </summary>
    public void ResetSprite()
    {
        StopAllCoroutines();
        sequenceIndex = 0;
        if (sequence != null && sequence.Length > 0)
        {
            SetSprite(sequence[sequenceIndex]);
        }
        isAnimating = false;
    }
}
