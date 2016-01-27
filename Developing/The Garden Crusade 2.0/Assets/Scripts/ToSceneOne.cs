using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToSceneOne : MonoBehaviour {

    public bool ActivateCanvas;

    public GameObject StartScreen;
    public GameObject OptionScreen;
    public GameObject CreditsScreen;
    public GameObject QuestScreen;
    public GameObject KeybindScreen;
    public GameObject MenuButton;
    public GameObject ExitToMenu;
    public GameObject deadScreen;
    public GameObject background;
    public GameObject loadingScreen;
    public GameObject general;
    public GameObject beetle;
    public Image loadingBar;
    public CanvasGroup loadGroup;
    public Animator loading;
    public Animator loadingR;
    public Image bar;
    public Image loadingL;
    public Image loadingRR;
    public Sprite L;
    public Sprite R;

    public Transform startPosition;
    public GameObject player;

    public void Start() {
        StartScreen.SetActive(true);
        OptionScreen.SetActive(false);
        CreditsScreen.SetActive(false);
        QuestScreen.SetActive(false);
        KeybindScreen.SetActive(false);
        ActivateCanvas = false;
        loadingScreen.SetActive(false);
    }

    public void FixedUpdate() {
        if (loadingBar.fillAmount < 1) {
            loadingBar.fillAmount += 1 * Time.deltaTime / 2.5f;           
        }
        player = GameObject.Find("Player");
    }

    public void OnLevelWasLoaded(int level) {
        switch (level) {
            case 0:
                print("4");
                background.SetActive(true);
            ActivateCanvas = false;
            OptionScreen.SetActive(false);
            MenuButton.SetActive(false);
            ExitToMenu.SetActive(false);
            StartScreen.SetActive(true);
            deadScreen.SetActive(false);
            GetComponent<AudioSource>().enabled = true;
                break;

            case 1:
                loadingScreen.SetActive(true);
            StartCoroutine(Loading());
            break;

            case 2:
                general = GameObject.Find("Termiet Generaal");
                general.SetActive(false);
            break;

            case 3:
                beetle = GameObject.Find("Vliegend Hert");
                beetle.SetActive(false);
            break;
        }
    }

    public void ChangeToScene() {
        Application.LoadLevel(1);
        loadingBar.fillAmount = 0;
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
            StartScreen.SetActive(true);
            OptionScreen.SetActive(false);
            CreditsScreen.SetActive(false);
            QuestScreen.SetActive(false);
            KeybindScreen.SetActive(false);
            ActivateCanvas = false;
            loadingScreen.SetActive(false);
        }
        if (Application.loadedLevel == 1) {
            StartScreen.SetActive(false);
            OptionScreen.SetActive(false);
            CreditsScreen.SetActive(false);
            QuestScreen.SetActive(false);
            KeybindScreen.SetActive(false);
            ActivateCanvas = false;
            if (Time.timeScale == 1.0F) {
                Time.timeScale = 0f;
            } else {
                if (Time.timeScale == 0f) {
                    Time.timeScale = 1.0f;
                }
            }
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

    IEnumerator Loading() {
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Canvas1").GetComponent<Canvas>().enabled = false;
        StartScreen.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        bar.enabled = false;
        background.SetActive(false);
        loadingL.GetComponent<Image>().sprite = L;
        loadingRR.GetComponent<Image>().sprite = R;
        loading.SetBool("Loading", true);
        loadingR.SetBool("Loading", true);

        yield return new WaitForSeconds(1f);

        MenuButton.SetActive(true);
        ExitToMenu.SetActive(true);   
        loadingScreen.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Canvas1").GetComponent<Canvas>().enabled = true;
        player.GetComponent<Quests>().InfoPauseGame();

    }
}

