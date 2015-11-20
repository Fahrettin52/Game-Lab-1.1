using UnityEngine;
using System.Collections;

public class Puzzle2 : MonoBehaviour {

	public int checkStenen;

	void Start () {
	
	}
	
	void Update () {
	
	}

	public void SteenCheck (){
		if(checkStenen == 4){
			print("Puzzle Complete");
		}
	}

}
