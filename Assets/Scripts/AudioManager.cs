// Author: Fatima Nadeem

using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundCategory[] categories; // SFX, music, system, etc

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (SoundCategory cat in categories)
        {
            foreach (Sound s in cat.sounds)
            {
                // Initialising audio source of sound
                s.audioSrc = gameObject.AddComponent<AudioSource>();
                s.audioSrc.clip = s.clip;
                s.audioSrc.volume = s.initialVol;
                s.audioSrc.loop = s.loop;
            }
        }
    }

    public void Play(string name)
    /*
        This function plays the sound of the given name regardless of category.
    */
    {     
        foreach (SoundCategory cat in categories)
        {
            Sound s = Array.Find(cat.sounds, sound => sound.name == name);
            
            if (s != null)
            {
                s.audioSrc.Play();
                return;
            }
        }
    }

    public void Stop(string name)
    /*
        This function stops the sound of the given name regardless of category.
    */
    {
        foreach (SoundCategory cat in categories)
        {
            Sound s = Array.Find(cat.sounds, sound => sound.name == name);

            if (s != null)
            {
                s.audioSrc.Stop();
                return;
            }
        }
    }

    public float GetCategoryVolume(string name)
    /*
        This function gets the volume of the category with the given name.
    */
    {
        SoundCategory cat = Array.Find(categories, c => c.name == name);

        if (cat != null)
        {
            return cat.GetVolume();
        }

        return 1f;
    }

    public void SetCategoryVolume(string name, float vol)
    /*
        This function sets the volume of the category with the given name.
    */
    {
        SoundCategory cat = Array.Find(categories, c => c.name == name);

        if (cat != null)
        {
            cat.SetVolume(vol);
        }
    }
}
