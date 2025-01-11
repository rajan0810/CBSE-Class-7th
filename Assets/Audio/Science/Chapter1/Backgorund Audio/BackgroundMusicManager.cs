using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Drag and drop the audio clips here.")]
    public AudioClip[] backgroundMusic; // Array to hold the audio clips
    [Tooltip("Duration of the fade in/out transitions (seconds).")]
    public float transitionDuration = 2.0f; // Duration of fade in/out transitions
    [Range(0f, 1f), Tooltip("Global volume for background music.")]
    public float musicVolume = 1.0f; // Music volume control

    private AudioSource audioSource1; // First audio source
    private AudioSource audioSource2; // Second audio source for crossfade
    private int currentTrackIndex = 0; // Index of the currently playing track
    private bool isPlayingFirstSource = true; // Tracks which AudioSource is currently playing

    void Start()
    {
        if (backgroundMusic.Length == 0)
        {
            Debug.LogError("No audio clips assigned to BackgroundMusicManager!");
            return;
        }

        // Create and configure two AudioSources
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource1.loop = false;
        audioSource2.loop = false;

        // Set initial volume
        audioSource1.volume = 0f;
        audioSource2.volume = 0f;

        // Start the music playback loop
        PlayNextTrack();
    }

    private void Update()
    {
        // Update the volume of both audio sources dynamically
        audioSource1.volume *= musicVolume;
        audioSource2.volume *= musicVolume;
    }

    private void PlayNextTrack()
    {
        // Determine the next track index
        currentTrackIndex = (currentTrackIndex + 1) % backgroundMusic.Length;

        // Get the next audio clip
        AudioClip nextClip = backgroundMusic[currentTrackIndex];

        // Swap audio sources and start crossfade
        if (isPlayingFirstSource)
        {
            StartCoroutine(Crossfade(audioSource1, audioSource2, nextClip));
        }
        else
        {
            StartCoroutine(Crossfade(audioSource2, audioSource1, nextClip));
        }

        // Toggle the active audio source
        isPlayingFirstSource = !isPlayingFirstSource;
    }

    private System.Collections.IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, AudioClip nextClip)
    {
        // Ensure the new audio source has the correct clip
        toSource.clip = nextClip;

        // Start playing the new audio source
        toSource.Play();

        // Gradually fade out the current source and fade in the new source
        float timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;

            fromSource.volume = Mathf.Lerp(musicVolume, 0f, t);
            toSource.volume = Mathf.Lerp(0f, musicVolume, t);

            yield return null;
        }

        // Ensure final volumes are set
        fromSource.volume = 0f;
        toSource.volume = musicVolume;

        // Stop the old audio source
        fromSource.Stop();

        // Schedule the next track
        Invoke(nameof(PlayNextTrack), nextClip.length - transitionDuration);
    }
}
