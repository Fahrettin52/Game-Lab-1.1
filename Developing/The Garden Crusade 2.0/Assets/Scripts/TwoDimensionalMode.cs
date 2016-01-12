using UnityEngine;
using System.Collections;

public class TwoDimensionalMode : MonoBehaviour {

	public GameObject mainCam;
	public GameObject twoDCam;

	void Start () {
		mainCam = GameObject.Find("MainCamera");
		twoDCam =  GameObject.Find("2DCamera"); 

		if(Application.loadedLevel == 3){
			GetComponent<Movement>().secondMode = true;
			mainCam.SetActive(false);
		}
		else{
            GetComponent<Movement>().secondMode = false;
            twoDCam.SetActive(false);
		}
	}
	
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {
		if(col.transform.name == "Switch"){
			print("Switch");
			GetComponent<Movement>().secondMode = false;
			mainCam.SetActive(true);
			twoDCam.SetActive(false);
		}
	}
}
