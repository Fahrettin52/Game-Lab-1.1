using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Puzzle1 : MonoBehaviour {
	public GameObject puzzleCanvas;
	public Image [] buttons;
	public GameObject mouseImage;
    public Sprite newSprite;
    public Sprite oldSprite;
    public bool[] checkPuzzle;
    public int currentButton;
	public bool completePuzzle;
    public GameObject enterTree;
    public AudioClip puzzleDone;
    public Animator sarahAnimator;

    void Start () {
        mouseImage = GameObject.Find("PuzzleMouse");
        puzzleCanvas = GameObject.Find("PuzzleActivateCanvas");
		puzzleCanvas.SetActive(false);
	}
	
	void Update () {
        oldSprite = newSprite;
        //DeactivatePuzzle ();
        AlphaZero ();
        //puzzleDone = GameObject.Find("SoundSource").GetComponent<SoundSource>().puzzleComplete;
    }

	public void ActivatePuzzle (){
		puzzleCanvas.SetActive(true);
		GetComponent<Movement>().enabled = false;
		GetComponent<Quests>().enabled = false;
		//GetComponent<Stamina>().enabled = false;
	}

	//void DeactivatePuzzle (){
	//	if(Input.GetButtonDown("Use")){
	//		puzzleCanvas.SetActive(false);
	//		GetComponent<Movement>().enabled = true;
	//		GetComponent<Quests>().enabled = true;
	//	}
	//}

	void AlphaZero (){
		if(mouseImage.GetComponent<Image>().sprite == null){
			mouseImage.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
		}
		else{
			mouseImage.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
		}
	}

    public void PlaceImage(int curA)
    {
        currentButton = curA;
        newSprite = buttons[currentButton].GetComponent<Image>().sprite;
        mouseImage.GetComponent<Image>().sprite = newSprite;
        buttons[currentButton].GetComponent<Image>().sprite = null;
        buttons[currentButton].GetComponent<Image>().sprite = oldSprite;
        PuzzleCompleted();
    }

    public void PuzzleCompleted(){
        if (buttons[0].GetComponent<Image>().sprite.name == "LadyBug5")
        {
            checkPuzzle[0] = true;
        }
        else
        {
            checkPuzzle[0] = false;
        }
        if (buttons[1].GetComponent<Image>().sprite.name == "LadyBug3")
        {
            checkPuzzle[1] = true;
        }
        else
        {
            checkPuzzle[1] = false;
        }
        if (buttons[2].GetComponent<Image>().sprite.name == "LadyBug8")
        {
            checkPuzzle[2] = true;
        }
        else
        {
            checkPuzzle[2] = false;
        }
        if (buttons[3].GetComponent<Image>().sprite.name == "LadyBug10")
        {
            checkPuzzle[3] = true;
        }
        else
        {
            checkPuzzle[3] = false;
        }

        if (checkPuzzle[0] == true && checkPuzzle[1] == true && checkPuzzle[2] == true && checkPuzzle[3] == true)
        {
            sarahAnimator.GetComponent<Animator>().SetBool("RunToIdle 0", false);
            //puzzleCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
            completePuzzle = true;
            puzzleCanvas.SetActive(false);
            GetComponent<Movement>().enabled = true;
            GetComponent<Quests>().enabled = true;
            mouseImage.GetComponent<Image>().sprite = null;
            GetComponent<Quests>().questText.SetActive(true);
            GetComponent<Quests>().popupText.SetActive(true);
            GetComponent<Quests>().popupText.SetActive(false);
            enterTree.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(puzzleDone);
        }
    }
}
