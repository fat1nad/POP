// Author: Fatima Nadeem

using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float initialVol; // Initial volume
    public bool loop; // Bool to dictate if this sound will loop

    [HideInInspector]
    public AudioSource audioSrc;
}
