using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("DialogueUI")] 
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("ChoicesUI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject firstChoice;
    private TextMeshProUGUI[] textChoices;

    private Story currentStory;
    public bool isPlaying;

    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) {
            Debug.LogWarning("More than one DialogueManager instance");
        }
    
        instance = this;
    }
    
    void Start() {
        isPlaying = false;
        dialogueUI.SetActive(false);

        textChoices = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) {
            textChoices[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    void Update()
    {
        if (!isPlaying) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            ContinueStory();
        }
    }

    public void EnterDialogue(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        isPlaying = true;
        dialogueUI.SetActive(true);
        choicePanel.SetActive(false);

        ContinueStory();
    }

    public IEnumerator ExitDialogue() {
        yield return new WaitForSeconds(0.2f);

        isPlaying = false;
        dialogueUI.SetActive(false);
        dialogueText.text = "";
        
    }

    public void ContinueStory() {
        if (currentStory.canContinue) {
            //Display next line
            dialogueText.text = currentStory.Continue();
            //Display choices
            DisplayChoices();
            // Clear eventsystem selection
            EventSystem.current.SetSelectedGameObject(null);
            // Set eventsystem selection
            EventSystem.current.SetSelectedGameObject(firstChoice);
        }
        else {
            StartCoroutine(ExitDialogue());
        }
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length) {
            Debug.LogError("More choices than supported");
        }

        int index = 0;
        foreach(Choice choice in currentChoices) {
            choicePanel.SetActive(true);
            choices[index].gameObject.SetActive(true);
            textChoices[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex); 
        choicePanel.SetActive(false);
    }
}
