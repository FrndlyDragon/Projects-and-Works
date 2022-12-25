using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool pause;

    public Dictionary<string, bool> triggers = new Dictionary<string, bool>();
    public Dictionary<string, int> stats;
    private List<string> trigNameList;
    private List<string> statNameList;
    private List<int> statValList;
    private string sceneName;

    void Start()
    {
        if (gameManager != null) {
            gameManager = null;
        }

        gameManager = this;
        pause = false;
        SceneManager.activeSceneChanged += updateScene;
        SceneManager.activeSceneChanged += createSave;
        tempTriggers(); 

        DontDestroyOnLoad(this.gameObject);
    }

    //Updates current scene
    private void updateScene(Scene scene, Scene next) {
        sceneName = next.name;
    }

    //Updates trigger dictionary
    public void updateTriggers(string triggerName, bool value) {
        if (triggers.ContainsKey(triggerName)) {
            triggers[triggerName] = value;
        }
        else {
            triggers.Add(triggerName, value);
        }
    }

    //Updates stats dictionary
    public void updateStats(string stat, int value) {
        stats[stat] = stats[stat] + value;
    }

    //Remember to delete
    private void tempTriggers() {
        triggers.Add("trigger1", true);
        triggers.Add("trigger2", true);
    }

    //Converts trigger dictionary into string list. Used in creating save
    private void convertTriggersToString(Dictionary<string, bool> dict) {
        List<string> triggers = new List<string>();
        foreach (KeyValuePair<string, bool> kvp in dict) {
            triggers.Add(kvp.Key);
        }

        trigNameList = triggers;
    }

    //Converts stats dictionary into string list and int list. Used in creating save
    private void convertStatsToString(Dictionary<string, int> dict) {
        List<string> statNames = new List<string>();
        List<int> statValues = new List<int>();
        foreach (KeyValuePair<string, int> kvp in dict) {
            statNames.Add(kvp.Key);
            statValues.Add(kvp.Value);
        }

        statNameList = statNames;
        statValList = statValues;
    }

    //Converts back into dictionary
    private void convertStringToTriggers(List<string> input) {
        foreach (string key in input) {
            triggers.Add(key, true);
        }
    }

    //Converts back into dictionary
    private void convertStringToStats(List<string> stat, List<int> statVal) {
        for (int i = 0; i < stat.Count; i++) {
            stats.Add(stat[i], statVal[i]);
        }
    }

    [System.Serializable]
    public class SaveObject {
        public List<string> trigNameList;
        public List<string> statNameList;
        public List<int> statValList;
        public string sceneName;
    }

    //Load save
    public void loadSave() {
        Debug.Log("Loading save");
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] files = info.GetFiles();

        if (files.Length < 0) {
            return;
        }

        string saveString = File.ReadAllText(Application.persistentDataPath + "/save.txt");
        SaveObject save = JsonUtility.FromJson<SaveObject>(saveString);

        convertStringToTriggers(save.trigNameList);
        convertStringToStats(save.statNameList, save.statValList);
        sceneName = save.sceneName;
    }

    //Creates save. Currently overwrites previous save. Will change once I get around to implementing multiple save files.
    public void createSave(Scene scene, Scene scene2) {
        Debug.Log("Creating save");
        convertStatsToString(stats);
        convertTriggersToString(triggers);

        SaveObject save = new SaveObject();
        save.sceneName = this.sceneName;
        save.trigNameList = this.trigNameList;
        save.statNameList = this.statNameList;
        save.statValList = this.statValList;

        string saveString = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + "/save.txt", saveString);
    }

    //Once multiple save files is implemented, will be used if loaded a save file.
    public void updateSave() {

    }
}
