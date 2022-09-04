using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Audio Management
    public AudioSource gameMusic;
    public Toggle mute;
    public Slider volumeAdjust;
    public Text volumeValue;
    public int lastVolume;

    // Opening menu
    public Button inSettingsExit;
    public GameObject inSettingsMenu;
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        gameMusic.volume = 1;
        gameMusic.loop = true;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Open Settings");
            //Check if settings is open
            if (!isOpen) {
                inOpenSettings();
                isOpen = true;
            }
            else if (isOpen) {
                inExitSettings();
                isOpen = false;
            }
        }
     
    }

    void inOpenSettings() {
        inSettingsExit.transform.localPosition = Vector3.zero;
        inSettingsMenu.transform.localPosition = Vector3.zero;
        inSettingsExit.interactable = true;
        Time.timeScale = 0;
    }

    void inExitSettings() {
        inSettingsExit.transform.Translate(0,600,0);
        inSettingsMenu.transform.Translate(0,600,0);
        inSettingsExit.interactable = false;
        Time.timeScale = 1;
    }

    // Possibly put this into separate script later
    public void muteAudio() {
        if (mute.isOn) {
            gameMusic.mute = true;
            lastVolume = (int)(gameMusic.volume * 100);
            volumeValue.text = "0";
        }
        else if (!mute.isOn){
            gameMusic.mute = false;
            volumeValue.text = lastVolume.ToString();
        }
        
    }

    public void adjustAudio() {
        gameMusic.volume = volumeAdjust.value / 100;
        volumeValue.text = volumeAdjust.value.ToString();

        if (volumeAdjust.value == 0f) {
            mute.isOn = true;
        }
        else {
            mute.isOn = false;            
        }
    }

}
