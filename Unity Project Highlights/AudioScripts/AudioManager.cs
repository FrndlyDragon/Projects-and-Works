using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSound[] sounds;
    public static AudioManager instance;

    void Awake() {
        instance = this;

        foreach (AudioSound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;

            s.source.clip = s.Clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void playSound(string name) {
        AudioSound s = Array.Find(sounds, sounds => sounds.Name == name);
        Debug.Log(s.source.clip);

        if (s == null) {
            Debug.LogWarning("Audio: " + name + " not found");
            return;
        }

        s.source.Play();
        Debug.Log("Playing: " + name);
    }

    public void adjustSound(string type, float value) {

        if (type == "SFX") {
            for (int i = 0; i < sounds.Length; i++) {
                if (sounds[i].soundType == type) {
                    sounds[i].source.volume = value;
                }
            }
            CharacterManager.instance.SFX(value);
        }

        else if (type == "MUS") {
            for (int i = 0; i < sounds.Length; i++) {
                if (sounds[i].soundType == type) {
                    sounds[i].source.volume = value;
                }
            }
            CharacterManager.instance.MUS(value);
        }

        else if (type == "MAS") {
            for (int i = 0; i < sounds.Length; i++) {
                sounds[i].source.volume = value;
            }
            CharacterManager.instance.MAS(value);
        }
    }
}
