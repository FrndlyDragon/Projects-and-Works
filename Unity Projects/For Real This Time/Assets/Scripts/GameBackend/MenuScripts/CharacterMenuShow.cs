using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Consolidate with CharacterMenuHide to create one CharacterMenu object
public class CharacterMenuShow : MonoBehaviour
{
    public Button thisButton;

    // HideMenu region around menu
    public Button target;

    // Menu Interface
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {
        thisButton.onClick.AddListener(showMenu);
    }

    public void showMenu() {
        // Move menu (might change to create interpolation of scale)
        target.transform.localPosition = Vector3.zero;
        menu.transform.localPosition = Vector3.zero;
        target.interactable = true;
        thisButton.interactable = false;

        // Pause Time (Implement full scale pause with bool later)
        Time.timeScale = 0;
    }
}
