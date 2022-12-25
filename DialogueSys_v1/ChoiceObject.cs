[System.Serializable]
public class ChoiceObject
{
    public string choiceText;
    public string triggerName;
    public string stat;
    public int statChange;
}

[System.Serializable]
public class ChoiceList 
{
    public ChoiceObject[] choiceOptions;
}
