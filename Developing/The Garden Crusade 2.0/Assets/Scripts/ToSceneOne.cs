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
    public Image loadingBar;
    private float time = 5;
    public CanvasGroup loadGroup;
    public float fadeSpeed;

    public Transform startPosition;

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
            loadingBar.fillAmount += 1 * Time.deltaTime / 4f;           
        } 
    }

    public void OnLevelWasLoaded(int level) {
        switch (level) {
            case 0:
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
        }
    }

    public void ChangeToScene() {
        Application.LoadLevel(1);
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

    IEnumerator Loading() {
        StartCoroutine("FadeIn");
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false; 
        yield return new WaitForSeconds(3);
        StartCoroutine("FadeOut");
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        StartScreen.SetActive(false);
        background.SetActive(false);
        MenuButton.SetActive(true);
        ExitToMenu.SetActive(true);
        loadingScreen.SetActive(false);
    }

    private IEnumerator FadeOut() {

        StopCoroutine("FadeIn");

        while (loadGroup.alpha > 0f) {

            float newValue = fadeSpeed * Time.deltaTime;

            if ((loadGroup.alpha - newValue) > 0f) {
                loadGroup.alpha -= newValue;
            } else {
                loadGroup.alpha = 0;
            }
            yield return null;
        }
    }

    private IEnumerator FadeIn() {

        StopCoroutine("FadeOut");

        while (loadGroup.alpha < 1f) {

            float newValue = fadeSpeed * Time.deltaTime;

            if ((loadGroup.alpha + newValue) < 1f) {
                loadGroup.alpha += newValue;
            } else {
                loadGroup.alpha = 1;
            }
            yield return null;
        }
    }
}

