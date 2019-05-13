using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBank : MonoBehaviour
{
    public Sound[] sounds;
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
    [Range(0, 1)] public float volume = 1;
    [Range(0, 1)] public float volumeVariance = 0.9f;

    [Range(-3, 3)] public float pitch = 1;
    [Range(-3, 3)] public float pitchVariance = 0.01f;

}
