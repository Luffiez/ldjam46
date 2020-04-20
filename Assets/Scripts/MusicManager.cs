using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance;
    [SerializeField]
    [Range(0f,1f)]
    float masterVolume = 1;
    float bgmVolume = 0.1f;
    float sfxVolume = 0.3f;
    [SerializeField]
    AudioSource bgmSource;
    [SerializeField]
    AudioSource sfxSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        bgmVolume *= masterVolume;
        sfxVolume *=  masterVolume;

        bgmSource.volume = bgmVolume; 
        sfxSource.volume = sfxVolume;

        if (bgmSource.clip != null)
            bgmSource.Play();

        bgmSource.volume = masterVolume;
    }

    public void ChangeBgmSong(AudioClip clip)
    {
        Debug.Log("Play: " + clip.name);
        bgmSource.clip = clip;
        bgmSource.Play();
    }


    public void ChangeBgmVolume(float volume)
    {
        bgmVolume = volume * masterVolume;
        bgmSource.volume = bgmVolume;
    }

    public void ChangeSfxVolume(float volume)
    {
        sfxVolume = volume * masterVolume;
        sfxSource.volume = sfxVolume;
    }

    public void ChangeMasterVolume(float volume)
    {
        masterVolume= volume;
    }

    public void StartMusic()
    {
        bgmSource.Play();
    }

    public void StopMusic()
    {
        bgmSource.Stop();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
}
