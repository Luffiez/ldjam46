using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance;
    [SerializeField]
    float Volume;
    AudioSource ASource;
    private void Awake()
    {
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
        ASource = GetComponent<AudioSource>();
        if (ASource.clip != null)
            ASource.Play();
        ASource.volume = Volume;
    }


    public void ChangeSong(AudioClip clip)
    {
        ASource.clip = clip;
        ASource.Play();
    }


    public void ChangeVolume(float volume)
    {
        ASource.volume = volume;
    }

    public void StartMusic()
    {
        ASource.Play();
    }

    public void StopMusic()
    {
        ASource.Stop();
    }


    public void PlayOneShot(AudioClip clip, float volume)
    {
        ASource.PlayOneShot(clip, volume);
    }
    // Start is called before the first frame update

}
