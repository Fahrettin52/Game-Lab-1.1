using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConversationSystem : MonoBehaviour {

	public GameObject convoCanvas;

	void Start () {
		convoCanvas = GameObject.Find("ConvoCanvas");
		convoCanvas.SetActive(false);
	}
	
	void Update () {
	
	}

	public void StartConvo (bool startConvo){
		if(startConvo == true){
			convoCanvas.SetActive(true);
		}
		else{
			convoCanvas.SetActive(false);
		}
	}
}
