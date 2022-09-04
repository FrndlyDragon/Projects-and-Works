using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Text hpText;
    public Player player;

    void Start() {
        hpText.text = "HP: " + player.health;
    }

    public void updateHealth() {
        hpText.text = "HP: " + player.health;
    }
}
