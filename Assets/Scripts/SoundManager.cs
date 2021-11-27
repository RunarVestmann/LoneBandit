using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioClip jumpSound;
    [Range(0f, 1f)] [SerializeField] float jumpSoundVolume;


    [SerializeField] AudioClip landingSound;
    [Range(0f, 1f)] [SerializeField] float landingSoundVolume;


    List<AudioSource> sources = new List<AudioSource>();

    void Awake()
    {
        var objects = GameObject.FindGameObjectsWithTag("SoundManager");
        if (objects.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        for (int i = 0; i < 5; i++)
        {
            sources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

    public void playJumpSound()
    {
        PlaySound(GetAvailableAudioSource(), jumpSound, jumpSoundVolume);
    }

    public void playLandingSound()
    {
        PlaySound(GetAvailableAudioSource(), landingSound, landingSoundVolume);
    }


    AudioSource GetAvailableAudioSource()
    {
        for (int i = 0; i < sources.Count; i++)
            if (!sources[i].isPlaying) return sources[i];
        var newSource = gameObject.AddComponent<AudioSource>();
        return newSource;
    }


    void PlaySound(AudioSource source, AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }
}

