using UnityEngine;
using System.Collections;

public class CheckStenen : MonoBehaviour {

	public int currentSteen;
	public GameObject puzzleManager;
	
	void Start () {
		puzzleManager = GameObject.Find("Puzzle 2");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter (Collider check){
		if(check.transform.name == "Steen Groen" && currentSteen == 1){
			puzzleManager.GetComponent<PuzzleUnderground>().puzzleCounter += 1;
		}
		if(check.transform.name == "Steen paars" && currentSteen == 2){
			puzzleManager.GetComponent<PuzzleUnderground>().puzzleCounter += 1;
		}
		if(check.transform.name == "Steen Rood" && currentSteen == 3){
			puzzleManager.GetComponent<PuzzleUnderground>().puzzleCounter += 1;
		}
		if(check.transform.name == "Steen Geel" && currentSteen == 4){
			puzzleManager.GetComponent<PuzzleUnderground>().puzzleCounter += 1;
		}
	}
}
