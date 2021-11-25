// Author: Fatima Nadeem

using UnityEngine;

[System.Serializable]
public class SoundCategory
{
    public string name;
    public Sound[] sounds;
    [Range(0f, 1f)]
    public float volume;

    public float GetVolume()
    {
        return volume;
    }

    public void SetVolume(float vol)
    {
        foreach (Sound s in sounds)
        {
            s.audioSrc.volume = s.initialVol * vol;
        }

        volume = vol;
    }
}
