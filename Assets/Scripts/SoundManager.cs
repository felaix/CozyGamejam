using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public Sound[] musicSounds, sfxSounds, atmosphereSounds;
    public AudioSource musicSource, sfxSource, atmosphereSource;
    public float fadeDuration = 1f;

    private int lastIndex;
   

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ! Play Music#
        PlayAtmosphere("atmo");
        PlayMusic("StandardMusic");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null) { Debug.Log($"Music {name} not found "); return; }

        int randomIndex = GetRandomIndex(s.clips.Length);

        while (GetIsLastIndex(randomIndex))
        {
            randomIndex = GetRandomIndex(s.clips.Length);
        }

        //musicSource.clip = s.clips[randomIndex];

        StartCoroutine(FadeMusic(s.clips[randomIndex]));
        //musicSource.Play(); 
        StoreIndex(randomIndex);
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null) { Debug.Log($"SFX {name} not found "); return; }

        sfxSource.Stop();

        sfxSource.PlayOneShot(s.clips[GetRandomIndex(s.clips.Length)]);
    }

    public void PlayAtmosphere(string name)
    {
        Sound s = Array.Find(atmosphereSounds, x => x.name == name);
        if (s == null) { Debug.LogError($"Atmo {name} not found"); return; }
        else { atmosphereSource.clip = s.clips[GetRandomIndex(s.clips.Length)]; atmosphereSource.Play(); }
    }

    public int GetRandomIndex(int length)
    {
        int randomIndex = UnityEngine.Random.Range(0, length);
        return randomIndex;
    }

    public void ModifyMusicVolume(Slider slider)
    {
        musicSource.volume = slider.value;
        atmosphereSource.volume = slider.value;
    }

    public void ModifySFXVolume(Slider slider)
    {
        sfxSource.volume = slider.value;
    }

    public void ModifyMasterVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }

    private void StoreIndex(int index) { lastIndex = index; }

    private bool GetIsLastIndex(int index) { if (index == lastIndex) return true; else return false; }

    private IEnumerator FadeMusic(AudioClip clip)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(1f,0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();

        musicSource.clip = clip;

        musicSource.Play();

        timer = 0f;

        while (timer < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0.0f, 1.0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
}
