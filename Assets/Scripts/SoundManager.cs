using System;
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ! Play Music#
        PlayMusic("StandardMusic");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null) { Debug.Log($"Music {name} not found "); return; }

        else { musicSource.clip = s.clips[GetRandomIndex(s.clips.Length)]; musicSource.Play(); }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null) { Debug.Log($"SFX {name} not found "); return; }

        sfxSource.Stop();

        sfxSource.PlayOneShot(s.clips[GetRandomIndex(s.clips.Length)]);
    }

    public int GetRandomIndex(int length)
    {
        int randomIndex = UnityEngine.Random.Range(0, length);
        return randomIndex;
    }

    public void ModifyMusicVolume(Slider slider)
    {
        musicSource.volume = slider.value;
    }

    public void ModifySFXVolume(Slider slider)
    {
        sfxSource.volume = slider.value;
    }

    public void ModifyMasterVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
}
