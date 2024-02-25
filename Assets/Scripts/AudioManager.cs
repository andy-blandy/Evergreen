using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Mixing")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource backgroundAmbience;
    public AudioSource backgroundMusic;

    [Header("Audio Clips")]
    public AudioClip forestAmbience;
    public AudioClip safeMusic;
    public AudioClip battleMusic;
    public AudioClip calmMusic;

    public static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        backgroundAmbience.clip = forestAmbience;

        /*
         * Uncomment the below functions if you want to test the audio
         */

        // PlayAmbience(true);

        PlaySafeMusic();

        // PlayBattleMusic();

        // PlayCalmMusic();
    }

    #region Play/Stop Functions
    public void PlayAmbience(bool setActive)
    {
        if (setActive)
        {
            backgroundAmbience.Play();
        } else
        {
            backgroundAmbience.Stop();
        }
    }
    public void PlayMusic(bool setActive)
    {
        if (setActive)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Stop();
        }
    }

    public void PlaySafeMusic()
    {
        backgroundMusic.clip = safeMusic;

        PlayMusic(true);
    }

    public void PlayBattleMusic()
    {
        backgroundMusic.clip = battleMusic;

        PlayMusic(true);
    }

    public void PlayCalmMusic()
    {
        backgroundMusic.clip = calmMusic;

        PlayMusic(true);
    }
    #endregion

    #region Adjust Vol Functions
    public void ChangeSFXVol(float vol)
    {
        audioMixer.SetFloat("SFX", vol);
    }

    public void ChangeMusicVol(float vol)
    {
        audioMixer.SetFloat("Music", vol);
    }

    public void ChangeMasterVol(float vol)
    {
        audioMixer.SetFloat("Master", vol);
    }
    #endregion
}
