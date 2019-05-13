using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    AudioSource[] audioSources;
    int currentIndexSource = 0;

    public void Initialize(int numberChannel)
    {
        for (int i = 0; i < numberChannel; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }
        audioSources = GetComponents<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip, float volume, float pitch)
    {
        audioSources[currentIndexSource].volume = volume;
        audioSources[currentIndexSource].pitch = pitch;
        audioSources[currentIndexSource].PlayOneShot(audioClip);

        currentIndexSource = (currentIndexSource + 1) % audioSources.Length;
    }
}
