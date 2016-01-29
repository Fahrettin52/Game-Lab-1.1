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

	public void ContinueConvoNPC (){
		if(GetComponent<InteractionWithEnvironment>().interactInt == 3){
			if(currentUnit < convoText.Length - 1){
				currentUnit += 1;
			}
			else{
				currentUnit = 0;
				startConvo = false;
            }
		}

	if(GetComponent<InteractionWithEnvironment>().interactInt == 2){
        if (currentUnit < 2 ){
		currentUnit += 1;
		}
		else{
			currentUnit = 0;
			startConvo = false;
		}
		}

        if (GetComponent<InteractionWithEnvironment>().interactInt == 5) {
            GameObject.Find("Moeder001FBX").GetComponent<Animator>().SetBool("idleToTalk", false);
        }
	}
}
