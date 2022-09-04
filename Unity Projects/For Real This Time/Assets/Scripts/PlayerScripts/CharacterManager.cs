using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    private void Awake() {
        Debug.Log("Awake");
        instance = this;
        SceneManager.sceneLoaded += LoadState;
    }

    //Resources
    private Weapon weapon;
    private SpriteRenderer weaponSkin;
    private SpriteRenderer playerSkin;

    //References
    public Player player;

    //Statistics
    private int coins;
    private int experience;
    private int level;

    //Game Data
    private string currScene;

    //Settings
    private float volumeMus;
    private float volumeSFX;
    private float volumeMas;

    public void SaveState() {
        coins = player.currCoins;
        experience = player.exp;
        level = player.level;
        //currScene = SceneManager.GetActiveScene().name;

        string s = "";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += level.ToString() + "|";
        //s += currScene + "|";

        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode) {
        if (!PlayerPrefs.HasKey("SaveState")) {
            Debug.Log("LoadState");
            return;
        }

        if (s.name == "DevScene") {
            PlayerPrefs.DeleteKey("SaveState");
            Debug.Log("LoadState");
            SceneManager.sceneLoaded -= LoadState;
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        player.currCoins = int.Parse(data[0]);
        player.exp = int.Parse(data[1]);
        player.level = int.Parse(data[2]);
        //SceneManager.LoadScene(data[3]);

        Debug.Log("LoadState");
        SceneManager.sceneLoaded -= LoadState;
    }

    public void SFX(float vol) {
        volumeSFX = vol;
    }

    public void MUS(float vol) {
        volumeMus = vol;
    }

    public void MAS(float vol) {
        volumeMus = vol;
    }
}
