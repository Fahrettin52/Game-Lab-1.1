using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ConversationSystem : MonoBehaviour {

	public GameObject convoCanvas;
	public bool startConvo;
	public string [] convoText;
	public GameObject text;
	public int currentUnit;

	void Start () {
		convoCanvas = GameObject.Find("ConvoCanvas");
		convoCanvas.SetActive(false);
	}
	
	void Update () {
		StartConvo ();
		Conversation ();
	}

	public void StartConvo (){
		if(startConvo == true){
			convoCanvas.SetActive(true);
		}
		else{
			convoCanvas.SetActive(false);
		}
	}

	public void Conversation (){
		text.GetComponent<Text>().text = convoText[currentUnit];
	}

	public void ContinueConvo (){
		if(currentUnit < convoText.Length){
			currentUnit += 1;
		}
		else{
			startConvo = false;
		}
	}
}
