using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class AudioSound {
    public string Name;
    public string soundType;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
