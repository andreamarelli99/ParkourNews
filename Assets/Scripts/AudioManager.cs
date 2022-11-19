using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

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

    private void OnEnable()
    {
        MusicVolume(0.05f);
        EventManager.StartListening("JumpSound", PlayJumpSound);
        EventManager.StartListening("DoubleJumpSound", PlayDoubleJumpSound);
        EventManager.StartListening("OnCoin", PlayCollectSound);
        EventManager.StartListening("JumpWallSound", PlayJumpWallSound);
        EventManager.StartListening("DeathSound", PlayDeathSound);
        EventManager.StartListening("SpawnSound", PlaySpawnSound);
        EventManager.StartListening("FinishSound", PlayFinishSound);
        EventManager.StartListening("DashSound", PlayDashSound);
        
    }

    private void PlayDashSound()
    {
        EventManager.StopListening("DashSound", PlayDashSound);
        
        PlaySound("DashSound");

        EventManager.StartListening("DashSound", PlayDashSound);
    }

    private void PlayFinishSound()
    {
        EventManager.StopListening("FinishSound", PlayFinishSound);
        
        PlaySound("FinishSound");

        EventManager.StartListening("FinishSound", PlayFinishSound);
    }

    private void PlaySpawnSound()
    {
        EventManager.StopListening("SpawnSound", PlaySpawnSound);
        
        PlaySound("DM-CGS-33");
        Debug.Log("Spawning sound");

        EventManager.StartListening("SpawnSound", PlaySpawnSound);
    }

    private void PlayDeathSound()
    {
        EventManager.StopListening("DeathSound", PlayDeathSound);
        
        PlaySound("DeathSound");

        EventManager.StartListening("DeathSound", PlayDeathSound);
    }

    private void PlayJumpWallSound()
    {
        EventManager.StopListening("JumpWallSound", PlayJumpWallSound);
        
        PlaySound("JumpWallSound");

        EventManager.StartListening("JumpWallSound", PlayJumpWallSound);
    }

    private void PlayCollectSound()
    {
        EventManager.StopListening("OnCoin", PlayCollectSound);
        
        PlaySound("CollectSound");

        EventManager.StartListening("OnCoin", PlayCollectSound);
    }

    private void PlayDoubleJumpSound()
    {
        EventManager.StopListening("DoubleJumpSound", PlayDoubleJumpSound);
        
        PlaySound("DoubleJumpSound");

        EventManager.StartListening("DoubleJumpSound", PlayDoubleJumpSound);
    }

    private void PlayJumpSound()
    {
        EventManager.StopListening("JumpSound", PlayJumpSound);
        
        PlaySound("JumpSound");

        EventManager.StartListening("JumpSound", PlayJumpSound);
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
