using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] int numberChannelPerGameObject = 6;
    Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
    Dictionary<GameObject, AudioSourceController> gameObjectDictionary = new Dictionary<GameObject, AudioSourceController>();

    void Awake ()
    {
        base.SetInstance(this);
        SetSoundBanksToDictionary();
    }

    void SetSoundBanksToDictionary()
    {
        SoundBank[] soundBanks = GetComponentsInChildren<SoundBank>();

        foreach(SoundBank soundBank in soundBanks)
        {
            for (int i = 0; i < soundBank.sounds.Length; i++)
            {
                Sound sound = soundBank.sounds[i];
                if (soundDictionary.ContainsKey(sound.name))
                {
                    Debug.LogError("Sound named <color=blue>" + sound.name + "</color> in soundbank named <color=green>" + soundBank.name + "</color> already exists");
                }
                else
                {
                    soundDictionary.Add(sound.name, sound);
                }
            }
        }
        Debug.Log("Sound Engine have " + soundDictionary.Count + " sounds");
    }

    public static void PlaySound(string name, GameObject gameObject)
    {
        if(!Instance.soundDictionary.ContainsKey(name))
        {
            Debug.LogError("The sound named <color=blue>" + name + "</color> doesn't exists.");
            return;
        }

        if(Instance.gameObjectDictionary.ContainsKey(gameObject))
        {
            AudioSourceController audioSourceControlle = Instance.gameObjectDictionary[gameObject];
            PlaySoundInternal(name, audioSourceControlle);
        }
        else
        {
            GameObject audioSource = new GameObject("Audio Source Controller");
            audioSource.transform.SetParent(gameObject.transform, true);

            AudioSourceController audioSourceController = audioSource.AddComponent<AudioSourceController>();
            audioSourceController.Initialize(Instance.numberChannelPerGameObject);
            Instance.gameObjectDictionary.Add(gameObject, audioSourceController);

            PlaySoundInternal(name, audioSourceController);
        }
    }

    private static void PlaySoundInternal(string name, AudioSourceController audioSourController)
    {
        Sound sound = Instance.soundDictionary[name];
        AudioClip audioClip = sound.clips[Random.Range(0, sound.clips.Length)];
        float volume = sound.volume + Random.Range(-sound.volumeVariance, sound.volumeVariance);
        float pitch = sound.pitch + Random.Range(-sound.pitchVariance, sound.pitchVariance);

        audioSourController.PlaySound(audioClip, volume, pitch);
    }

}
