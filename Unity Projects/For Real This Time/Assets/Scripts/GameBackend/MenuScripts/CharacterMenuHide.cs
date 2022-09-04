using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuHide : MonoBehaviour
{
    public Button thisButton;
    public Button menuButton;
    public GameObject menu;

    void Update() {
        thisButton.onClick.AddListener(hideMenu);
    }

    public void hideMenu() {
        transform.Translate(0,600,0);
        thisButton.interactable = false;
        menu.transform.Translate(0,600,0);
        menuButton.interactable = true;

        //Resume Time
        Time.timeScale = 1;
    }
}
