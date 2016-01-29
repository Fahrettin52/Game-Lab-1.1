using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TermiteGeneral : MonoBehaviour {

 private static TermiteGeneral instance;

    public static TermiteGeneral Instance {
        get{
            if (instance == null) {
                instance = FindObjectOfType<TermiteGeneral>();
            }
            return instance;
        }
    }

    public Animator animator;
    public Transform player;
	public float distance;
	public float range;
	public float attackRange;
	public float moveSpeed;
    public int livesEnemy;
    public GameObject[] dropRandomItem;
    public Image fill;
    public NavMeshAgent agent;
    private bool resetBool;
    public float navSpeed;
    public float maxVert, minVert, maxHor, minHor;
    public float countToRoam, countToRoamMax;
    public bool mayRoam;
    public Vector3 toRoam;
    public bool idle;
    public bool mayDie = false;
    private bool mayDrop = true;
    public GameObject spinning;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player").transform;

        if(idle==false){
        navSpeed = GetComponent<NavMeshAgent>().speed;
        }
	}

	void Update () {
		transform.position = new Vector3(transform.position.x, 469.3f, transform.position.z);
        if (idle == false) {
            Roam();
            EnemyFollow();
        }
	}

	void EnemyFollow (){
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);
            if (distance <= 35 && distance > attackRange) {
                animator.SetBool("Threat", true);
                agent.Stop();
            }
            else {
                animator.SetBool("Threat", false);
                agent.Resume();
            }

            if (distance < range){
                FollowPlayer();
            }
            else {
                Roam();
                resetBool = true;
            }

            if (distance < attackRange){
                animator.SetTrigger("MayAttack");
                agent.Stop();
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            }
            else {
                agent.Resume();
            }
        }
	}

	void FollowPlayer (){

        agent.SetDestination(player.transform.position);
        if (resetBool == true) {
            transform.LookAt(agent.destination);
            resetBool = false;
        }
    }

	public void DropDead (int damage){
        livesEnemy -= damage;
        fill.fillAmount -= (damage / 100f);
        if( livesEnemy < 1) {
            livesEnemy = 0; 
        }
        if (livesEnemy == 0) {
            mayDie = true;
            spinning.SetActive(true);
        }
        if (livesEnemy == 0 && mayDie == true && mayDrop == true){
            mayDie = false;  
            GameObject.Find("_Manager").GetComponent<ToSceneOne>().general = null;
            GameObject.Find("Player").GetComponent<Quests>().currentObjective += 1;
			GameObject.Find("Player").GetComponent<Quests>().currentObjectiveText += 1;
			GameObject.Find("Player").GetComponent<Quests>().LoopForBool ();
            GameObject.Find("Player").GetComponent<Experience>().currentExp += GameObject.Find("Player").GetComponent<Experience>().expGet;
            animator.SetTrigger("MayDie");
            Destroy(gameObject, 1f);
            GameObject.Instantiate(dropRandomItem[Random.Range(0, 4)]).transform.position = transform.position;
            mayDrop = false;
        }
    }
    
    public void Roam() {
        countToRoam -= 1 * Time.deltaTime;
        if(countToRoam<=0) {
            toRoam = new Vector3(Random.Range(maxHor, minHor), transform.position.y + 4.5f, Random.Range(maxVert, minVert));
            agent.SetDestination(toRoam);
            agent.Resume();
            countToRoam = countToRoamMax;
        }   
    }
}

