using UnityEngine;
using System.Collections;

public class InteractionWithEnvironment : MonoBehaviour {

	public float rayDis;
	public RaycastHit rayHit;
	public int interactInt;
	public bool convoActive;
	
	void Start () {

	}
	
	void Update (){
		ShootRay ();
	}

	void Interaction (int CountInteract) {
		CountInteract = interactInt;
		switch(CountInteract){
			case 1:
				print("Puzzle Level 1");
				break;
			case 2:
				convoActive = true;
				GetComponent<ConversationSystem>().StartConvo(convoActive);
				break;
			case 3:
				print("NPC");
				break;
			case 4:
				print("Shrink ray");
				break;
			case 5:
				print("Puzzle Level 3");
				break;
		}
	}

	void ShootRay (){
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Tutorial Puzzle"){
				if(Input.GetButtonDown("Use")){
					interactInt = 1;
					Interaction (interactInt);
				}
			}
		}
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Mother"){
				if(Input.GetButtonDown("Use")){
					interactInt = 2;
					Interaction (interactInt);
				}
			}
		}
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, rayDis)){
			if(rayHit.transform.tag == "NPC"){
				if(Input.GetButtonDown("Use")){
					interactInt = 3;
					Interaction (interactInt);
				}
			}
		}
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Shrink ray"){
				if(Input.GetButtonDown("Use")){
					interactInt = 4;
					Interaction (interactInt);
				}
			}
		}
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Puzzle Underground"){
				if(Input.GetButtonDown("Use")){
					interactInt = 5;
					Interaction (interactInt);
				}
			}
		}
	}
}
