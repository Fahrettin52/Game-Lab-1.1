using UnityEngine;
using System.Collections;

public class TwoDimensionalMode : MonoBehaviour {

	void Start () {
		if(Application.loadedLevel == 3){
			GetComponent<Movement>().secondMode = true;
		}
	}
	
	void Update () {
	
	}
}
