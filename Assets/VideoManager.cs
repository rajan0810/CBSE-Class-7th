/*using UnityEngine;

public class VideoScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/

using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign your Video Player in Inspector
    public VideoClip[] videoClips;  // Array of videos
    
    private int currentVideoIndex = 0; 

    void Start()
    {
        if (videoClips.Length > 0)
        {
            PlayVideo(currentVideoIndex);
        }
    }

    public void PlayVideo(int index)
    {
        if (index >= 0 && index < videoClips.Length)
        {
            videoPlayer.clip = videoClips[index];  // Assign selected video
            videoPlayer.Play(); // Start playing
            currentVideoIndex = index; // Update current video index
        }
        else
        {
            Debug.LogError("Invalid video index!");
        }
    }

    public void PlayNextVideo()
    {
        int nextIndex = (currentVideoIndex + 1) % videoClips.Length;
        PlayVideo(nextIndex);
    }

    public void PlayPreviousVideo()
    {
        int prevIndex = (currentVideoIndex - 1 + videoClips.Length) % videoClips.Length;
        PlayVideo(prevIndex);
    }
}

