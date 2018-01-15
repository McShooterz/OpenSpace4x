/*****************************************************************************************************************************************
Author: Michael Shoots
Email: michael.shoots@live.com
Project: Open Space 4x
License: MIT License
Notes:
******************************************************************************************************************************************/

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [HideInInspector]
    public AudioSource uiSource = null;
    [HideInInspector]
    public AudioSource musicSource = null;
    [HideInInspector]
    public float MasterVolume = 0.5f;
    [HideInInspector]
    public float MusicVolume = 0.5f;
    [HideInInspector]
    public float EffectsVolume = 0.5f;
    [HideInInspector]
    public float UIVolume = 0.5f;

    int soundEffectCount = 0;

    void Awake()
    {
        //Make a Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        if(uiSource == null)
            uiSource = gameObject.AddComponent<AudioSource>();
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        //This stays in every scene
        DontDestroyOnLoad(gameObject);
    }

    public void PlayUIClip(string clipName)
    {
        AudioClip clip = ResourceManager.instance.GetAudioClip(clipName);
        if (clip != null)
        {
            uiSource.pitch = 1f;
            uiSource.PlayOneShot(clip, MasterVolume * UIVolume);
        }
        else
        {
            print("audio clip not found: " + clipName);
        }
    }

    public void PlayEffectClip(AudioSource source, string clipName, bool AlwaysPlay)
    {
        if (source != null && CanPlaySoundEffect() || AlwaysPlay)
        {
            AudioClip clip = ResourceManager.instance.GetAudioClip(clipName);
            if (clip != null)
            {
                source.pitch = GetRandomPitch();
                source.PlayOneShot(clip, GetEffectsVolume());
                soundEffectCount++;
                if(soundEffectCount == 1)
                    StartCoroutine(ResetCanPlaySoundEffect());
            }
            else
            {
                print("audio clip not found: " + clipName);
            }
        }
    }

    public void AddEffectClip(GameObject parent, string clipName, bool AlwaysPlay)
    {
        if (CanPlaySoundEffect() || AlwaysPlay)
        {
            AudioClip clip = ResourceManager.instance.GetAudioClip(clipName);
            if (clip != null)
            {
                AudioSource source = parent.AddComponent<AudioSource>();
                source.pitch = GetRandomPitch();
                source.PlayOneShot(clip, GetEffectsVolume());
                soundEffectCount++;
                if (soundEffectCount == 1)
                    StartCoroutine(ResetCanPlaySoundEffect());
            }
            else
            {
                print("audio clip not found: " + clipName);
            }
        }    
    }

    public void PlayRandomEffectClip(AudioSource source, params string[] clipNames)
    {
        if (CanPlaySoundEffect() && source != null)
        {
            string clipName = clipNames[Random.Range(0, clipNames.Length)];
            AudioClip clip = ResourceManager.instance.GetAudioClip(clipName);
            if (clip != null)
            {
                source.pitch = GetRandomPitch();
                source.PlayOneShot(clip, GetEffectsVolume());
                soundEffectCount++;
                if (soundEffectCount == 1)
                    StartCoroutine(ResetCanPlaySoundEffect());
            }
            else
            {
                print("audio clip not found: " + clipName);
            }
        }
    }

    float GetEffectsVolume()
    {
        return EffectsVolume * MasterVolume;
    }

    public void UpdateMusicVolume()
    {
        musicSource.volume = MusicVolume * MasterVolume;
    }

    public void UpdateUIVolume()
    {
        uiSource.volume = UIVolume * MasterVolume;
    }

    float GetRandomPitch()
    {
        return Random.Range(0.95f, 1.05f);
    }

    bool CanPlaySoundEffect()
    {
        return soundEffectCount < 3;
    }

    IEnumerator ResetCanPlaySoundEffect()
    {
        yield return new WaitForSeconds(0.1f);
        soundEffectCount = 0;
    }
}