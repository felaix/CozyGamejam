using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        // ! Play Music
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null) { Debug.LogError($"Music {name} not found"); return; }

        else { musicSource.clip = s.clips[GetRandomIndex(s.clips.Length)]; musicSource.Play(); }
        //else { musicSource.clip = s.clips[]}
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null) { Debug.LogError($"SFX {name} not found"); return; }

        sfxSource.Stop();

        sfxSource.PlayOneShot(s.clips[GetRandomIndex(sfxSounds.Length)]);
    }

    public void ModifyMusicVolume(Slider slider)
    {
        musicSource.volume = slider.value;
    }

    public void ModifySFXVolume(Slider slider)
    {
        sfxSource.volume = slider.value;
    }

    public int GetRandomIndex(int length) { return UnityEngine.Random.Range(0, length); }

}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
}
