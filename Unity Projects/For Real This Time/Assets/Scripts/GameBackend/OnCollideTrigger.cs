using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideTrigger : MonoBehaviour
{
    public bool playerInside;
    public bool objectInside;
    private void OnTriggerEnter2D(Collider2D coll) {
        Debug.Log(coll);
        if (coll.CompareTag("Player")) {
            playerInside = true;
        }
        objectInside = true;
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.CompareTag("Player")) {
            playerInside = false;
        }
        objectInside = false;
    }
}
