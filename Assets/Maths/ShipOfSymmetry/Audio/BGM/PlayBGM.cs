using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayBGM : MonoBehaviour
{
    public bool Pause;

    public AudioSource[] sourceList;
    AudioSource currentSource;
    int currentIndex;


    void Start()
    {
        currentIndex = 0;
        currentSource = sourceList[currentIndex];
        for (int i = 0; i< sourceList.Length; i++)
        {
            sourceList[i].volume = 0;
        }
    }

    void Update()
    {
        // if (Pause)
        // {
        //     if (currentSource.isPlaying)
        //     {
        //         currentSource.Pause();
        //         Debug.Log("Audio Paused");
        //     }
        // }
        // else
        // {
        //     if (!currentSource.isPlaying)
        //     {
        //         currentSource.UnPause();
        //         Debug.Log("Audio Resumed");
        //     }
        // }


        if (!currentSource.isPlaying)
        {
            currentSource = sourceList[currentIndex];
            currentIndex = (currentIndex + 1) % sourceList.Length;
            currentSource.Play();
            StartCoroutine(Fade(true, currentSource, 3f, 1f));
            StartCoroutine(Fade(false, currentSource, 3f, 0f));
        }
    }

    public IEnumerator Fade(bool fadeIn, AudioSource source, float duration, float targetVolume)
    {
        if (!fadeIn)
        {
            double los  = (double) source.clip.samples / source.clip.frequency;
            yield return new WaitForSecondsRealtime((float) los - duration);
        }

        float time = 0;
        float startVol = source.volume;
        while (time < duration)
        {
            // if (Pause) // Suspend fading when paused
            // {
            //     yield return new WaitUntil(() => !Pause);
            // }

            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol,targetVolume, time / duration);
            yield return null;
        }
        yield break;
    }
}
