using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Include this for LINQ methods

public class Ath3na : MonoBehaviour
{
    public List<Dialogue> dialogues = new List<Dialogue>();
    public AudioSource audioSource;
    public Animator animator;

    public void Speak(string dialogueName)
    {
        // Find the dialogue with the specified name
        Dialogue dialogue = dialogues.FirstOrDefault(d => d.id == dialogueName);

        if (dialogue != null)
        {
            Debug.Log(dialogue.dialogText);

            if (audioSource != null && dialogue.dialogAudioClip != null)
            {
                audioSource.clip = dialogue.dialogAudioClip;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Dialogue with name '{dialogueName}' not found.");
        }
    }
}
