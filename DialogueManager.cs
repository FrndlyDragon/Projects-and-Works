using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; //Instance variable

    [SerializeField]
    private GameObject dialogueUI, dialogueMenuUI, dialogueChoicesUI;
    [SerializeField]
    private Button choice1, choice2, choice3;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private float dialogueSpeed;
    [SerializeField]
    private TextMeshProUGUI tmpText;
    [SerializeField]
    private string startNode;

    //Text
    private Dictionary<string, DialogueNode> dialogueNodes;
    private DialogueNode currentNode;

    //Booleans
    private bool isDialogue;
    private bool isAnimating;
    private bool isChoices;

    //Animation Params and Variables
    private int lineCount;
    private int charCount;
    private int[] lineCharCount;

    //Getters
    public bool _IsDialogue { get {return isDialogue;}}
    public bool _IsAnimating { get {return isAnimating;}}
    public Dictionary<string, DialogueNode> _dialogueNodes {get {return dialogueNodes;} set {dialogueNodes = value;}}
    
    void Awake()
    {
        if (instance != null) {
            instance = null;
        }

        instance = this;
        isDialogue = false;
        isChoices = false;
        dialogueUI.SetActive(false);
        dialogueChoicesUI.SetActive(false);
        dialogueMenuUI.SetActive(false);
        isAnimating = false;
    }

    void Start() {
        foreach (KeyValuePair<string, DialogueNode> kvp in dialogueNodes) {
            Debug.Log(kvp.Value.nodeOrder);
        }
        if (dialogueNodes.Count > 0) {
            startDialogue();
            currentNode = dialogueNodes[startNode];
            Debug.Log(currentNode.dialogue);
            writeText(currentNode.dialogue); 
        }

    }

    void Update() {
        if (isChoices || isAnimating || GameManager.gameManager.pause) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && isDialogue) {
            if (!isAnimating) {
                if (currentNode.nextNode.Count > 0) {
                    DialogueNode temp = determineNextNode(currentNode);
                    if (temp == null) {
                        endDialogue();
                        return;
                    }
                    currentNode = temp;
                    Debug.Log(currentNode.dialogue);
                    writeText(currentNode.dialogue);
                }
                else {
                    endDialogue();
                }
            }
        }
    }

    public void startDialogue() {
        if (isDialogue) {
            return;
        }
        Debug.Log("Starting dialogue");
        isDialogue = true;
        dialogueUI.SetActive(true);
    }

    public void endDialogue() {
        if (!isDialogue) {
            return;
        }
        Debug.Log("Ending dialogue");
        isDialogue = false;
        dialogueUI.SetActive(false);
    }

    public void writeText(string text) {
        spriteChange(currentNode);
        tmpText.text = text;
        tmpText.ForceMeshUpdate();

        lineCount = tmpText.textInfo.lineCount;
        charCount = tmpText.textInfo.characterCount;

        lineCharCount = new int[lineCount];

        for (int i = 0; i < lineCount; i++) {
            lineCharCount[i] = tmpText.textInfo.lineInfo[i].characterCount;
        }

        tmpText.maxVisibleCharacters = 0;
        tmpText.maxVisibleLines = 1;
        StartCoroutine(textAnimation());
    } 

    private IEnumerator textAnimation() {
        Debug.Log("Starting animation");
        isAnimating = true;
        int currentLine = 0;
        int currentChar = 0;
        float scrollInterval = 1/(lineCount - 3);

        while(tmpText.maxVisibleCharacters < tmpText.textInfo.characterCount) {
            tmpText.maxVisibleCharacters += 1;
            if (lineCharCount[currentLine] + currentChar == tmpText.maxVisibleCharacters) {
                currentLine++;
                tmpText.maxVisibleLines++;

                if (tmpText.maxVisibleLines > 3) {
                    scrollRect.verticalNormalizedPosition -= scrollInterval;
                }

                currentChar += tmpText.maxVisibleCharacters;
            }
            yield return new WaitForSeconds(dialogueSpeed);
        }

        displayChoice();
        isAnimating = false;
    }

    private void displayChoice() {
        displayChoices(currentNode);
    }

    private DialogueNode determineNextNode(DialogueNode currentNode) {
        bool isNextNode = false;

        foreach (string next in currentNode.nextNode) {
            for (int i = 0; i < dialogueNodes[next].triggers.Length; i++) {
                string key = dialogueNodes[next].triggers[i];
                Debug.Log(key);

                if (GameManager.gameManager.triggers.ContainsKey(key)) {
                    isNextNode = true;
                    continue;
                }
                else {
                    isNextNode = false;
                    break;
                }
            }

            if (isNextNode == true) {
                Debug.Log(next);
                return dialogueNodes[next];
            }
        }

        foreach (string next in currentNode.nextNode) {
            if (dialogueNodes[next].triggers.Length == 0) {
                return dialogueNodes[next];
            }
        }

        if (isNextNode == false) {
            Debug.Log("Next node not found");
            return null;
        }
        else {
            return null;
        }
    }

    private void displayChoices(DialogueNode currentNode) {
        Button[] buttons = {choice1, choice2, choice3};
        if (currentNode.choices.choiceOptions != null) {
            isChoices = true;
            dialogueChoicesUI.SetActive(true);
            for (int i = 0; i < currentNode.choices.choiceOptions.Length; i++) {
                ChoiceObject tempChoice = currentNode.choices.choiceOptions[i];
                Button tempButton = buttons[i];

                tempButton.GetComponentInChildren<TMP_Text>().text = tempChoice.choiceText;

                tempButton.onClick.AddListener(delegate{GameManager.gameManager.updateTriggers(tempChoice.triggerName, true);});
                if (tempChoice.stat.Length > 0) {
                    tempButton.onClick.AddListener(delegate{GameManager.gameManager.updateStats(tempChoice.stat, tempChoice.statChange);});
                }
                tempButton.onClick.AddListener(closeChoices);

                tempButton.interactable = true;
                tempButton.gameObject.SetActive(true);
            } 
        }
    }

    private void closeChoices() {
        choice1.onClick.RemoveAllListeners();
        choice2.onClick.RemoveAllListeners();
        choice3.onClick.RemoveAllListeners();

        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        choice3.gameObject.SetActive(false);

        dialogueChoicesUI.SetActive(false);
        isChoices = false;

        if (currentNode.nextNode.Count > 0) {
            DialogueNode temp = determineNextNode(currentNode);
            if (temp == null) {
                endDialogue();
                return;
            }
            currentNode = temp;
            Debug.Log(currentNode.dialogue);

            writeText(currentNode.dialogue);
            displayChoices(currentNode);
        }
        else {
            endDialogue();
        }
    }

    private void spriteChange(DialogueNode currentNode) {
        if (currentNode.npcSprite != null) {
            currentNode.npcTarget.sprite = currentNode.npcSprite;
        }
    }
}
