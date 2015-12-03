using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Quests : MonoBehaviour {

	public int quest1_1;
	public int currentObjective;
	public int currentObjectiveText = 0;
	public int currentTag = 0;
	public int energyShards;
	public int shrinkRayActivate;
	public int oldValues;
	public int oldValuesText;

	public bool [] quest1;
	public string [] quest1Text;
	public string [] tagManagerQuest1;

	public GameObject boomstronkLevel;
	public GameObject undergroundLevel;
	public GameObject schuurLevel;
	public GameObject tutLevel;
	public GameObject popupText;
	public GameObject puzzleCanvas;
	public Text textTest;
    public int teller;
    public GameObject questText;
    public GameObject puzzleHelper;

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
                    puzzleHelper.SetActive(true);
                    questText.SetActive(false);
                    popupText.SetActive(false);
					print("ActivatePuzzle");
					GetComponent<Puzzle1>().ActivatePuzzle ();
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
        if(quest1[5] == true) {
            quest1[3] = false;
            quest1[4] = false;
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

		if(Physics.Raycast(transform.position + new Vector3 (0, 1.3f, 0), transform.forward, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Mother" && quest1[9] == true){
                Debug.DrawRay(transform.position, transform.forward);
				popupText.SetActive(true);
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
	
	public void LoopForBool (){
		for(int i = teller; i < quest1.Length; i ++){
			if(i == currentObjective){
					quest1[i] = true;
			}
		}
	}
}
