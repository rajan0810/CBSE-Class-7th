using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Ath3na : MonoBehaviour
{
    private bool active;
    public List<Dialogue> dialogues = new List<Dialogue>();
    public AudioSource audioSource;
    public Animator animator;
    public TextMeshProUGUI captionText; // For UI caption display
    public float captionDisplayTime = 5f; // Duration to show text

    public void Speak(string dialogueName)
    {
        // Find the dialogue with the specified name
        Dialogue dialogue = dialogues.FirstOrDefault(d => d.id == dialogueName);

        if (dialogue != null)
        {
            Debug.Log(dialogue.dialogText);

            // Play the audio if available
            if (audioSource != null && dialogue.dialogAudioClip != null)
            {
                audioSource.clip = dialogue.dialogAudioClip;
                audioSource.Play();
            }

            // Display the caption
            if (captionText != null)
            {
                StopAllCoroutines(); // Ensure previous captions are cleared
                StartCoroutine(DisplayCaption(dialogue.dialogText));
            }
        }
        else
        {
            Debug.LogWarning($"Dialogue with name '{dialogueName}' not found.");
        }
    }

    private System.Collections.IEnumerator DisplayCaption(string text)
    {
        captionText.text = text;
        yield return new WaitForSeconds(captionDisplayTime);
        captionText.text = ""; // Clear text after duration
    }

    public void Spawn(Transform spawnLocation)
    {
        if (!active)
        {
            transform.position = spawnLocation.position;
            transform.rotation = spawnLocation.rotation;
            gameObject.SetActive(true);
            active = true;
            Debug.Log("Ath3na has spawned at the target location.");
        }
    }

    public void Despawn()
    {
        if (active)
        {
            gameObject.SetActive(false);
            active = false;
            Debug.Log("Ath3na has despawned.");
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.SetTrigger(animationName);
            Debug.Log($"Playing animation: {animationName}");
        }
        else
        {
            Debug.LogWarning("Animator is not assigned.");
        }
    }
}
