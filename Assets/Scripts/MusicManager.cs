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
    float sfxVolume = 0.6f;
    [SerializeField]
    AudioSource bgmSource;
    [SerializeField]
    AudioSource sfxSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip menuClickClip;

    public float BgmVolume { get { return bgmVolume * masterVolume; } private set => bgmVolume = value; }
    public float SfxVolume { get { return sfxVolume * masterVolume; } private set => sfxVolume = value; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        bgmSource.volume = BgmVolume; 
        sfxSource.volume = SfxVolume;

        if (bgmSource.clip != null)
            bgmSource.Play();

        bgmSource.volume = masterVolume;
    }

    public void ChangeBgmSong(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PauseSfx(bool pause)
    {
        if (pause)
            sfxSource.Pause();
        else
            sfxSource.Play();
    }

    public void ChangeBgmVolume(float volume)
    {
        BgmVolume = volume * masterVolume;
        bgmSource.volume = BgmVolume;
    }

    public void ChangeSfxVolume(float volume)
    {
        sfxSource.volume = SfxVolume;
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

    public void PlayMenuButton()
    {
        sfxSource.PlayOneShot(menuClickClip);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clip">The audioclip to play</param>
    /// <param name="volumeModifier">Modifier for normal sfx volume. 0f - 1f</param>
    public void PlayOneShot(AudioClip clip, float volumeModifier = 1f)
    {
        sfxSource.PlayOneShot(clip, SfxVolume * volumeModifier);
    }
}
