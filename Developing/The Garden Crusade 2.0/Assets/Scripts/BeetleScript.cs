using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeetleScript : MonoBehaviour {

	private static BeetleScript instance;

    public static BeetleScript Instance {
        get{
            if (instance == null) {
                instance = FindObjectOfType<BeetleScript>();
            }
            return instance;
        }
    }

	public Transform player;
	public float distance;
	public float range;
	public float attackRange;
	public float moveSpeed;
    public int livesEnemy;
    public GameObject dropRandomItem;
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
    public Animator animator;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player").transform;

        if(idle==false){
        navSpeed = GetComponent<NavMeshAgent>().speed;
        }
	}

	void Update () {
		transform.position = new Vector3(transform.position.x, 342.36f, transform.position.z);
        if (idle == false) {
            Roam();
            EnemyFollow();
        }
	}

	void EnemyFollow (){
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);
            if (distance < range){
                animator.SetBool("mayWalk", true);
                FollowPlayer();
            }
            else {
                Roam();
                animator.SetBool("mayWalk", false);
                resetBool = true;
            }

            if (distance < attackRange){
                agent.Stop();
                transform.LookAt(player);
                animator.SetBool("mayAttack", true);
                animator.SetBool("mayWalk", false);
            }
            else {
                agent.Resume();
                animator.SetBool("mayAttack", false);
                animator.SetBool("mayWalk", true);
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
        fill.fillAmount -= (damage / 1000f);
        if( livesEnemy < 1) {
            livesEnemy = 0; 
        }
        if (livesEnemy == 0) {
            mayDie = true;
        }
        if (livesEnemy == 0 && mayDie == true && mayDrop == true){
            mayDie = false;  
            GameObject.Find("_Manager").GetComponent<ToSceneOne>().beetle = null;
            GameObject.Find("Player").GetComponent<Quests>().currentObjective += 1;
			GameObject.Find("Player").GetComponent<Quests>().currentObjectiveText += 1;
			GameObject.Find("Player").GetComponent<Quests>().LoopForBool ();
            GameObject.Find("Player").GetComponent<Experience>().currentExp += GameObject.Find("Player").GetComponent<Experience>().expGet;
            animator.SetBool("mayDie", true);
            animator.SetBool("mayAttack", false);
            animator.SetBool("mayWalk", false);
            Destroy(gameObject, 1f);
            GameObject.Instantiate(dropRandomItem).transform.position = transform.position;
            mayDrop = false;
        }
    }
    
    public void Roam() {
        countToRoam -= 1 * Time.deltaTime;
        if(countToRoam<=0) {
           toRoam = new Vector3(Random.Range(maxHor, minHor), transform.position.y, Random.Range(maxVert, minVert));
            agent.SetDestination(toRoam);
            agent.Resume();
            countToRoam = countToRoamMax;
        }   
    }
}
