using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public TextAsset nodeJson; 
    public string nodeName;
    public int nodeOrder;

    public string dialogue;
    public string npcName;
    public string npcSpriteName;
    public Sprite npcSprite;
    public Image npcTarget;
    public List<string> nextNode;
    public string[] triggers;
    public ChoiceList choices = new ChoiceList();

    public DialogueNode(string nodeName, string dialogue, string npcName, string npcSprite, int nodeOrder, string[] triggers) {
        this.nodeName = nodeName;
        this.nodeOrder = nodeOrder;
        this.dialogue = dialogue;
        this.npcName = npcName;
        this.npcSpriteName = npcSprite;
        this.triggers = triggers;
    }
}
