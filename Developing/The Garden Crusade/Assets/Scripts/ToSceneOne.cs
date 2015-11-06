using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToSceneOne : MonoBehaviour {

    public GameObject StartScreen;
    public GameObject OptionScreen;
    public GameObject CreditsScreen;
    public GameObject QuestScreen;
    public GameObject KeybindScreen;
    public GameObject MenuButton;
    public GameObject ExitToMenu;
    public bool ActivateCanvas;
    public GameObject deadScreen;

    public Transform startPosition;

    public void Start() {
        StartScreen.SetActive(true);
        OptionScreen.SetActive(false);
        CreditsScreen.SetActive(false);
        QuestScreen.SetActive(false);
        KeybindScreen.SetActive(false);
        ActivateCanvas = false;
    }
    public void OnLevelWasLoaded(int level) {
        switch (level) {
            case 0:
            ActivateCanvas = false;
            OptionScreen.SetActive(false);
            //StartScreen = GameObject.Find("StartScreen");
            MenuButton.SetActive(false);
            ExitToMenu.SetActive(false);
            StartScreen.SetActive(true);
            deadScreen.SetActive(false);
            GetComponent<AudioSource>().enabled = true;
                break;

            case 1:
            MenuButton.SetActive(true);
            StartScreen.SetActive(false);
            //StartScreen = GameObject.Find("Filler");
            ExitToMenu.SetActive(true);
                // 1 stuff
               // startPosition = GameObject.Find("StartPosition").transform;
              //  PlayerScript.Instance.transform.position = startPosition.position;
                break;
        }
    }

    public void ChangeToScene() {
        Application.LoadLevel(1);
        OnLevelWasLoaded(1);
    }

    public void ClickExit() {
        Application.Quit();
    }

    public void OpenOptions() {
        StartScreen.SetActive(false);
        OptionScreen.SetActive(true);
        CreditsScreen.SetActive(false);
        QuestScreen.SetActive(false);
        KeybindScreen.SetActive(false);
        ActivateCanvas = true;
    }

    public void Return() {
        if (Application.loadedLevel == 0) {
            Start();
        } 
        else {
            StartScreen.SetActive(false);
            OptionScreen.SetActive(false);
            CreditsScreen.SetActive(false);
            QuestScreen.SetActive(false);
            KeybindScreen.SetActive(false);
            ActivateCanvas = false;
        }
    }

    public void Credits() {
        StartScreen.SetActive(false);
        CreditsScreen.SetActive(true);
    }
    public void OpenOptionsInGame() {
        OptionScreen.SetActive(!OptionScreen.activeInHierarchy);

        if (ActivateCanvas == false) {
            ActivateCanvas = true;
        } else {
            ActivateCanvas = false;
        }
    }
    public void OpenQuest() {
        OptionScreen.SetActive(false);
        QuestScreen.SetActive(true);
        if (ActivateCanvas) {
            ActivateCanvas = true;
        } else {
            ActivateCanvas = false;
        }
    }
    public void OpenKeyBind() {
        KeybindScreen.SetActive(true);
        OptionScreen.SetActive(false);
        ActivateCanvas = false;

    }
    public void ReturnToOption() {
        OptionScreen.SetActive(true);
        KeybindScreen.SetActive(false);
        ActivateCanvas = true;
    }
    public void ExitToStart() {
        Application.LoadLevel(0);
        OnLevelWasLoaded(0);
    }
}

