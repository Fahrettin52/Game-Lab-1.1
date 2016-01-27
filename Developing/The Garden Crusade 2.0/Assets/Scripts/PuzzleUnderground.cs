using UnityEngine;
using System.Collections;

public class PuzzleUnderground : MonoBehaviour {

	public int puzzleCounter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PuzzleDone();
	}

	public void PuzzleDone (){
		if(puzzleCounter == 4){
			GameObject.Find("Player").GetComponent<Quests>().currentObjective += 1;
			GameObject.Find("Player").GetComponent<Quests>().currentObjectiveText += 1;
			GameObject.Find("Player").GetComponent<Quests>().LoopForBool ();
			Application.LoadLevel("Level3");
			puzzleCounter = 5;
		}
	}
}
