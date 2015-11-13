using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Puzzle1 : MonoBehaviour {
	public GameObject puzzleCanvas;
	public GameObject [] buttons;
	public GameObject mouseImage;
	public Sprite newSprite;
	public Sprite oldSprite;

	public int currentButton;
	public bool completePuzzle;

	void Start () {
		 mouseImage = GameObject.Find("PuzzleMouse");
		 puzzleCanvas = GameObject.Find("PuzzleActivateCanvas");
		 puzzleCanvas.SetActive(false);
	}
	
	void Update () {
		oldSprite = newSprite;
		DeactivatePuzzle ();
		AlphaZero ();
		PuzzleCompleted ();
	}

	public void ActivatePuzzle (){
		puzzleCanvas.SetActive(true);
		GetComponent<Movement>().enabled = false;
		GetComponent<Quests>().enabled = false;
	}

	void DeactivatePuzzle (){
		if(Input.GetButtonDown("Use")){
			puzzleCanvas.SetActive(false);
			GetComponent<Movement>().enabled = true;
			GetComponent<Quests>().enabled = true;
		}
	}

	void AlphaZero (){
		if(mouseImage.GetComponent<Image>().sprite == null){
			mouseImage.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
		}
		else{
			mouseImage.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
		}
	}

	void SwitchImage (){
		for(int i = 0; i < buttons.Length; i ++){
			if(i == currentButton){
				newSprite = buttons[i].transform.Find("Image").GetComponent<Image>().sprite;
				mouseImage.GetComponent<Image>().sprite = newSprite;
				buttons[i].transform.Find("Image").GetComponent<Image>().sprite = null;
					if(buttons[i].transform.Find("Image").GetComponent<Image>().sprite == null){
					buttons[i].transform.Find("Image").GetComponent<Image>().sprite = oldSprite;
					}
			}
		}
		
	}

	public void ButtonClick1 (){
		currentButton = 0;
		print(newSprite);
		SwitchImage ();
	}

	public void ButtonClick2 (){
		currentButton = 1;
		print(newSprite);
		SwitchImage ();
	}

	public void ButtonClick3 (){
		currentButton = 2;
		print(newSprite);
		SwitchImage ();
	}

	public void ButtonClick4 (){
		currentButton = 3;
		print(newSprite);
		SwitchImage ();
	}

	void PuzzleCompleted (){
		if(buttons[0].transform.Find("Image").GetComponent<Image>().sprite.name == "LadyBug1" /*&& 
		   buttons[1].transform.Find("Image").GetComponent<Image>().sprite.name == "LadyBug2" && 
		   buttons[2].transform.Find("Image").GetComponent<Image>().sprite.name == "LadyBug3" && 
		   buttons[3].transform.Find("Image").GetComponent<Image>().sprite.name == "LadyBug4"*/){
			completePuzzle = true;

		}
	}
}