using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("InteractUI")]
    [SerializeField] private GameObject interactUI;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool inRange;
    public bool isOpen;

    void Awake() {
        inRange = false;
        interactUI.SetActive(false);
    }

    void Update() {
        if (inRange && !DialogueManager.instance.isPlaying) {
            interactUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F)) {
                DialogueManager.instance.EnterDialogue(inkJSON);
            }
        }
        else {
            interactUI.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            inRange = false;
        }
    }
}
