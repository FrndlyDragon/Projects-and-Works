using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCollision : Collision
{
    protected bool collected = false;
    public int coinGranted;

    protected override void OnCollision(Collider2D coll) {
        if (collected) {
            Debug.Log("AlreadyCollected");
        } 

        else if (coll.gameObject.tag == "Player") {
            collected = true;
            Debug.Log("MoneyCollected:" + coinGranted);
            coll.SendMessage("addCoin", coinGranted);
        }

    }
}
