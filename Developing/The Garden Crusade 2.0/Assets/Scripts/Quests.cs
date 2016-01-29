using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Quests : MonoBehaviour {

    public Animator sarahAnimator;
    public int quest1_1;
	public int currentObjective;
	public int currentObjectiveText = 0;
	public int currentTag = 0;
	public int energyShards;
	public int shrinkRayActivate;
	public int oldValues;
	public int oldValuesText;

	public bool [] quest1;
	private bool levelLoaded, levelLoaded2;
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
    //public Texture puzzleSwap;

    public RaycastHit rayHit;
	public float rayDis;
    public GameObject scrollHeal;
    public Transform grotlevelPos;
    public GameObject soundToOpenQuest;

    void Awake () {

		if(Application.loadedLevel == 2){
            teller = 7;
            boomstronkLevel = GameObject.Find("Boomstronk Level");
			boomstronkLevel.SetActive(false);
			print("Level 666");
			
        }

		if(Application.loadedLevel == 3){
			schuurLevel = GameObject.Find("Schuur Level");
			schuurLevel.SetActive(false);
		}

		popupText = GameObject.Find("Canvas1/Pop up text");
		popupText.SetActive(false);

		if(Application.loadedLevel == 4){
			undergroundLevel = GameObject.Find("Underground Level");
			undergroundLevel.SetActive(false);
        }
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
                if (Input.GetButtonDown("Use")){
                    sarahAnimator.GetComponent<Animator>().SetFloat("idleToRun 0", 0f);
                    puzzleHelper.SetActive(true);
                    questText.SetActive(false);
                    popupText.SetActive(false);
                    //puzzleCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    GetComponent<Puzzle1>().ActivatePuzzle ();
					currentObjective += 1;
					currentObjectiveText += 1;
					LoopForBool ();
                    //GameObject.Find("Puzzle").GetComponent<Renderer>().material.mainTexture = puzzleSwap;
                    //scrollHeal.SetActive(true);
                    quest1[5] = false;
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
                    GetComponent<InteractionWithEnvironment>().interactInt = 2;
                    GetComponent<InteractionWithEnvironment>().Interaction(2);
                    //Destroy(GameObject.FindWithTag("Mother"));
                }
			}
			else{
				popupText.SetActive(false);
			}
		}

		if(quest1[10] == true){
            GameObject.Find("Spawn").GetComponent<SpawnEnemy>().SpawnTrippleAttack();
			GameObject.Find("_Manager").GetComponent<ToSceneOne>().general.SetActive(true);
			quest1[10] = false;
		}
     }

	void LevelTwoQuests () {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "NPC" && quest1[14] == true){
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

        if (Application.loadedLevel == 4 && levelLoaded == false) {
            grotlevelPos = GameObject.Find("Player Position").GetComponent<Transform>();
            transform.position = grotlevelPos.position;
            GetComponent<SummoningInsects>().enabled = true;
            levelLoaded = true;
            print(levelLoaded);
        }

        if(Application.loadedLevel != 4){
        	 GetComponent<SummoningInsects>().enabled = false;
        }
	}

	void LevelThreeQuests () {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Shrink ray" && quest1[20] == true || quest1[21] == true || quest1[22] == true || quest1[23] == true || quest1[24] == true){
				popupText.SetActive(true);
				if(Input.GetButtonDown("Use")){
					currentObjective += 1;
					currentObjectiveText += 1;
					shrinkRayActivate += 1;
					quest1[20] = false;
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

		if(quest1[20] == true){
			if(energyShards == 3){
				quest1[21] = true;
				currentObjective += 1;
				currentObjectiveText += 1;
				LoopForBool ();
				energyShards = 4;
			}
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
            GetComponent<Puzzle1>().enabled = false;
            GetComponent<Quests>().boomstronkLevel = GameObject.Find("Boomstronk Level");
            //GetComponent<Stamina>().enabled = true;
            boomstronkLevel.SetActive(true);
		}

		/*if(trigger.transform.tag == "Start Underground" && quest1[12] == true){
			undergroundLevel.SetActive(true);
		}*/

		/*if(trigger.transform.tag == "StartSchuur" && quest1[18] == true){
			schuurLevel.SetActive(true);
		}*/

		if(trigger.transform.tag == "Exit City" == true){
            if ( quest1[11] == true) {
                Application.LoadLevel("Level 2");
                quest1[11] = false;
            }
            else {
                GameObject.Find("Player").GetComponent<PlayerScript>().currentHealth = 0;
            }
		}

		//if(trigger.transform.tag == "Enemy"){
		//	quest1_1 += 1;
		//	Destroy(trigger.gameObject);
		//}

		if(trigger.transform.tag == "Shards"){
			energyShards += 1;
			Destroy(trigger.gameObject);
			
		}

		if(trigger.transform.tag == "Black Widow"){
			quest1[19] = true;
		}
	}
	
	public void LoopForBool (){
		for(int i = teller; i < quest1.Length; i ++){
			if(i == currentObjective){
                quest1[i] = true;
                if ( currentObjective < 8) {
                    QuestCompleteSound();
                    InfoPauseGame();
                }
			}
		}
	}
    public void QuestCompleteSound() {
        Instantiate(soundToOpenQuest, transform.position, transform.rotation);
    }
    
    public void InfoPauseGame() {
        GameObject.Find("ContinueButton").GetComponent<Image>().enabled = true;
        GameObject.Find("ContinueText").GetComponent<Text>().enabled = true;
        Time.timeScale = 0f;
    }
    public void UnPauseClick() {
        GameObject.Find("ContinueButton").GetComponent<Image>().enabled = false;
        GameObject.Find("ContinueText").GetComponent<Text>().enabled = false;
        Time.timeScale = 1f;
    }
}

