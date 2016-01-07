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
			print("Puzzle Done");
		}
	}
}
