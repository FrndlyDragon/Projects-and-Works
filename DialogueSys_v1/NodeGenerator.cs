using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [System.Serializable]
    public class TempNode {
        public string nodeName;
        public int nodeOrder;
        public string dialogue;
        public string npcName;
        public string npcSprite;
        public string[] triggers;
    }

    public DialogueNode[] dialogueNodes;
    private Dictionary<string, DialogueNode> dialogueNodeDict = new Dictionary<string, DialogueNode>(); 
    private ChoiceList choices = new ChoiceList();
    private TempNode temp;
    void Awake()
    {
        foreach (DialogueNode node in dialogueNodes) {
            Debug.Log("Converting");
            //Debug.Log(node.nodeJson.text);
            temp = JsonUtility.FromJson<TempNode>(node.nodeJson.text);
            //Debug.Log(temp.dialogue);
            choices = JsonUtility.FromJson<ChoiceList>(node.nodeJson.text);
            //Debug.Log(choices);
            node.dialogue = temp.dialogue;
            node.nodeName = temp.nodeName;
            node.nodeOrder = temp.nodeOrder;
            node.npcName = temp.npcName;
            node.npcSpriteName = temp.npcSprite;
            node.triggers = temp.triggers;
            node.choices = choices;
        }

        SortNodes(dialogueNodes);
        
        foreach (DialogueNode node in dialogueNodes) {
            dialogueNodeDict.Add(node.nodeName, node);
        }

        DialogueManager.instance._dialogueNodes = dialogueNodeDict;
    }

    private void SortNodes(DialogueNode[] dialogueNodes) {
        foreach (DialogueNode node in dialogueNodes) {
            for (int i = 0; i < dialogueNodes.Length; i++) {
                if (dialogueNodes[i].nodeOrder == node.nodeOrder + 1) {
                    node.nextNode.Add(dialogueNodes[i].nodeName);
                }
            }
        }
    }
}
