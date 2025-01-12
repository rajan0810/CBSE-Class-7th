using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XRAccess.Chirp;

[System.Serializable]
public class CaptionData
{
    [TextArea]
    public string captionText; // The text to display as a caption
    public AudioClip audioClip; // Corresponding audio clip
    public int DialogNumber; // Dialog number to trigger the caption
}

public class NarratorAudioPlayer : MonoBehaviour
{
    public void Start(){
        callDialog(1);
    }
    public CaptionData[] captions; // Array of captions
    public AudioSource audioSource; // Audio source for playing audio
    public CaptionSource captionSource; // Caption source for displaying captions

    /// <summary>
    /// Method to start the dialog playback by dialog number.
    /// </summary>
    /// <param name="dialogNumber">The dialog number to start.</param>
    public void callDialog(int dialogNumber)
    {
        foreach (CaptionData data in captions)
        {
            if (data.DialogNumber == dialogNumber)
            {
                // Initialize the sentence and word count lists
                List<string> sentenceList = new List<string>();
                List<int> wordCountList = new List<int>();

                // Split the caption text into sentences
                string[] sentences = data.captionText.Split('.');
                foreach (string sentence in sentences)
                {
                    string trimmedSentence = sentence.Trim();
                    if (!string.IsNullOrEmpty(trimmedSentence))
                    {
                        // Add the sentence to the list
                        sentenceList.Add(trimmedSentence);

                        // Count the number of words in the sentence
                        int wordCount = trimmedSentence.Split(' ').Length;
                        wordCountList.Add(wordCount);
                    }
                }

                // Start the audio and captions coroutine
                StartCoroutine(PlayDialogWithCaptions(data.audioClip, sentenceList, wordCountList));
                break;
            }
        }
    }

    /// <summary>
    /// Coroutine to play audio and show captions sequentially.
    /// </summary>
    /// <param name="audioClip">Audio clip to play.</param>
    /// <param name="sentenceList">List of sentences for captions.</param>
    /// <param name="wordCountList">List of word counts corresponding to sentences.</param>
    /// <returns></returns>
    private IEnumerator PlayDialogWithCaptions(AudioClip audioClip, List<string> sentenceList, List<int> wordCountList)
    {
        // Play the audio clip
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        // Display each caption with timing
        for (int i = 0; i < sentenceList.Count; i++)
        {
            // Debug.Log(sentenceList[i]);
            // Debug.Log(wordCountList[i]);
            captionSource.ShowTimedCaption(sentenceList[i], wordCountList[i] * 0.35f);
            yield return new WaitForSeconds(wordCountList[i] * 0.35f); // Wait for the caption duration
        }
    }
}
