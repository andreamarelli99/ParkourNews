using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] public AudioClip[] musicSounds, sfxSounds;
    [SerializeField] public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string sound)
    {
        AudioClip s = Array.Find(musicSounds, x => x.name == sound);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = s;
            musicSource.Play();
        }
    }
    
    public void PlaySound(string sound)
    {
        AudioClip s = Array.Find(sfxSounds, x => x.name == sound);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s);
        }
    }

    public void ToogleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    
    public void ToogleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    
    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
