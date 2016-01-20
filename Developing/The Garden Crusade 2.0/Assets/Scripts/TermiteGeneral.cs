using UnityEngine;
using System.Collections;

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

	public Transform player;
	public float distance;
	public float range;
	public float attackRange;
	public float moveSpeed;
    public int livesEnemy = 100;
    public GameObject dropRandomItem;
    public Image fill;
    public NavMeshAgent agent;
    private bool resetBool;
    public float navSpeed;
    public float maxVert, minVert, maxHor, minHor;
    public float countToRoam, countToRoamMax;
    public bool mayRoam;
    public Vector3 toRoam;
    public bool mayDie = false;
    private bool mayDrop = true;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player").transform;

        if(idle==false){
        navSpeed = GetComponent<NavMeshAgent>().speed;
        }

        attackRange = 5f;
	}

	void Update () {
        if (idle == false) {
            //Roam();
            EnemyFollow();
        }
	}

	void EnemyFollow (){
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);
            if (distance < range){
                FollowPlayer();
            }
            else {
                Roam();
                resetBool = true;
            }

            if (distance < attackRange){
                agent.Stop();
                transform.LookAt(player);
                //GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(GetComponent<SoundSource>().playerDamageTaking);
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
        }
        if (livesEnemy == 0 && mayDie == true && mayDrop == true){
            print("Dead2");
            mayDie = false;  
            GameObject.Find("Player").GetComponent<Experience>().currentExp += GameObject.Find("Player").GetComponent<Experience>().expGet;
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

