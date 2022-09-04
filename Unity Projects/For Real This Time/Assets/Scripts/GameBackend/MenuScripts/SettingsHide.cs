using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHide : MonoBehaviour
{
    public GameObject settingMenu;
    public Settings settings;
    public Button settingBackground;

    public void hideSetting() {
        settingBackground.transform.Translate(0,600,0);
        settingMenu.transform.Translate(0,600,0);
        settingBackground.interactable = false;
        Time.timeScale = 1;
        settings.isOpen = false;
    }
}
