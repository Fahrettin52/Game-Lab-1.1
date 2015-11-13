using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Quests : MonoBehaviour {

	private int quest1_1;
	public int currentObjective;
	public int currentObjectiveText = 0;
	public int currentTag = 0;
	public int energyShards;
	public int shrinkRayActivate;

	public bool [] quest1;
	public string [] quest1Text;
	public string [] tagManagerQuest1;

	public GameObject boomstronkLevel;
	public GameObject undergroundLevel;
	public GameObject schuurLevel;
	public GameObject tutLevel;
	public GameObject popupText;
	public Text textTest;
    public int teller;

	public RaycastHit rayHit;
	public float rayDis;

	void Start () {
		if(Application.loadedLevel == 2){
            teller = 7;
            boomstronkLevel = GameObject.Find("Boomstronk Level");
			boomstronkLevel.SetActive(false);
		}

		if(Application.loadedLevel == 3){
			undergroundLevel = GameObject.Find("Underground Level");
			undergroundLevel.SetActive(false);
		}

		popupText = GameObject.Find("Canvas1/Pop up text");
		popupText.SetActive(false);

		if(Application.loadedLevel == 4){
			schuurLevel = GameObject.Find("Schuur Level");
			schuurLevel.SetActive(false);
		}

		/*if(Application.loadedLevel == 0){
			tutLevel = GameObject.Find("Tutorial Level");
			tutLevel.SetActive(false);
		}*/
	}
	
	
	void Update () {

		LevelOneQuests ();
		LevelTwoQuests ();
		LevelThreeQuests ();
		TutorialQuests ();

	}

	void TutorialQuests () {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Tutorial Puzzle" && quest1[5] == true){
				popupText.SetActive(true);
				if(Input.GetButtonDown("Use")){
                    //GetComponent<Puzzle1>().ActivatePuzzle();
                    Destroy(rayHit.transform.gameObject);
					currentObjective += 1;
					currentObjectiveText += 1;
					LoopForBool ();
				}
			}
			else{
				popupText.SetActive(false);
			}
		}
	}

	void LevelOneQuests () {
		textTest.text = quest1Text[currentObjectiveText];
		if(quest1_1 == 5){
			quest1[currentObjective] = true;
			currentObjectiveText += 1;
			if(currentObjectiveText > 1){
				quest1_1 = 6;
			}
		}

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Mother" && quest1[9] == true){
				popupText.SetActive(true);
				print("Show Text");
				if(Input.GetButtonDown("Use")){
					currentObjective += 1;
					currentObjectiveText += 1;
					LoopForBool ();
					Destroy(rayHit.transform.gameObject);
				}
			}
			else{
				popupText.SetActive(false);
			}
		}

	}

	void LevelTwoQuests () {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "NPC" && quest1[13] == true){
				popupText.SetActive(true);
				if(Input.GetButtonDown("Use")){
					currentObjective += 1;
					currentObjectiveText += 1;
					LoopForBool ();
					Destroy(rayHit.transform.gameObject);
				}
			}
		}
		else{
			popupText.SetActive(false);
		}
	}

	void LevelThreeQuests () {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Shrink ray" && quest1[18] == true){
				popupText.SetActive(true);
				if(Input.GetButtonDown("Use")){
					currentObjective += 1;
					currentObjectiveText += 1;
					shrinkRayActivate += 1;
					quest1[18] = false;
					LoopForBool ();
					if(shrinkRayActivate == 3){
						Destroy(rayHit.transform.gameObject);
					}
				}
			}
		}
		else{
			popupText.SetActive(false);
		}
	}

	void OnTriggerEnter (Collider trigger){
		for(int i = 0; i < tagManagerQuest1.Length; i ++){
			if(trigger.transform.tag == tagManagerQuest1[currentTag] && quest1[currentObjective] == true){
				currentObjective += 1;
				currentObjectiveText += 1;
				currentTag += 1;
                LoopForBool();
                Destroy(trigger.gameObject);
				break;
			}
		}
		if(trigger.transform.tag == "Start Tutorial"){
			tutLevel.SetActive(true);
			currentObjectiveText += 1;
			LoopForBool ();
			Destroy(trigger.gameObject);
		}

		if(trigger.transform.tag == "Start Boomstronk"	&& quest1[7] == true){
			boomstronkLevel.SetActive(true);
		}

		if(trigger.transform.tag == "Start Underground" && quest1[11] == true){
			undergroundLevel.SetActive(true);
		}

		if(trigger.transform.tag == "StartSchuur" && quest1[17] == true){
			schuurLevel.SetActive(true);
		}

		if(trigger.transform.tag == "Enemy"){
			quest1_1 += 1;
			Destroy(trigger.gameObject);
		}

		if(trigger.transform.tag == "Energy Shard" && quest1[19] == true){
			energyShards += 1;
			Destroy(trigger.gameObject);
			if(energyShards == 3){
				quest1[18] = true;
				currentObjective += 1;
				currentObjectiveText += 1;
				LoopForBool ();
			}
		}

		if(trigger.transform.tag == "Black Widow"){
			quest1[18] = true;
		}
	}
	
	void LoopForBool (){
		for(int i = teller; i < quest1.Length; i ++){
			if(i == currentObjective){
					quest1[i] = true;
			}
		}
	}
}
