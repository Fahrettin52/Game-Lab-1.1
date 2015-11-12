using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Puzzle1 : MonoBehaviour {
	public GameObject puzzleCanvas;
	public GameObject [] buttons;
	public GameObject mouseImage;
	public Sprite newSprite;
	public Sprite oldSprite;

	void Start () {
		 mouseImage = GameObject.Find("PuzzleMouse");
		 puzzleCanvas = GameObject.Find("PuzzleActivateCanvas");
		 puzzleCanvas.SetActive(false);
	}
	
	void Update () {
		DeactivatePuzzle ();
		AlphaZero ();
		oldSprite = newSprite;
	}

	public void ActivatePuzzle (){
		print("PuzzleActivate");
		puzzleCanvas.SetActive(true);
		GetComponent<Movement>().enabled = false;
		GetComponent<Quests>().enabled = false;
	}

	void DeactivatePuzzle (){
		if(Input.GetButtonDown("Use")){
			print("Connect");
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

	public void SwitchImage1 (){
		mouseImage.GetComponent<Image>().sprite = buttons[0].GetComponent<Image>().sprite;
		buttons[0].GetComponent<Image>().sprite = newSprite;
		buttons[0].GetComponent<Image>().sprite = null;
	}
}