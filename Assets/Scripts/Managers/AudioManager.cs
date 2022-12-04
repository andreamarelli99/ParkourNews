using System;
using System.Collections;
using ParkourNews.Scripts;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] public AudioClip[] musicSounds, sfxSounds;
    [SerializeField] public AudioSource musicSource, sfxSource;

    private DataManager _dataManager;

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
        _dataManager = FindObjectOfType<DataManager>();
        musicSource.mute = !_dataManager.GetMusicEnabled();
        musicSource.volume = _dataManager.GetMusicVolume();
        sfxSource.mute = !_dataManager.GetSfxEnabled();
        sfxSource.volume = _dataManager.GetSfxVolume();
    }

    private void OnEnable()
    {
        EventManager.StartListening("JumpSound", PlayJumpSound);
        EventManager.StartListening("DoubleJumpSound", PlayDoubleJumpSound);
        EventManager.StartListening("OnCoin", PlayCollectSound);
        EventManager.StartListening("JumpWallSound", PlayJumpWallSound);
        EventManager.StartListening("DeathSound", PlayDeathSound);
        EventManager.StartListening("SpawnSound", PlaySpawnSound);
        EventManager.StartListening("FinishSound", PlayFinishSound);
        EventManager.StartListening("DashSound", PlayDashSound);
        EventManager.StartListening("WalkingSound", PlayWalkingSound);
    }

    private void PlayWalkingSound()
    {
        EventManager.StopListening("WalkingSound", PlayWalkingSound);

        PlaySound("WalkingSound");

        StartCoroutine(CanWalkSoundCoroutine());
    }

    private IEnumerator CanWalkSoundCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        EventManager.StartListening("WalkingSound", PlayWalkingSound);
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

        PlaySound("SpawnSound");
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
        var s = Array.Find(musicSounds, x => x.name == sound);

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
        var s = Array.Find(sfxSounds, x => x.name == sound);

        if (s == null)
            Debug.Log("Sound not found");
        else
        {
            sfxSource.clip = s;
            sfxSource.Play();
        }
    }

    public void EnableMusic(bool enable)
    {
        musicSource.mute = enable;
        _dataManager.SetMusicEnabled(enable);
    }

    public void EnableSfx(bool enable)
    {
        sfxSource.mute = enable;
        _dataManager.SetSfxEnabled(enable);
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        _dataManager.SetMusicVolume(volume);
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
        _dataManager.SetSfxVolume(volume);
    }
}