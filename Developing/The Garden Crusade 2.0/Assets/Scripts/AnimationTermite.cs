using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimationTermite : MonoBehaviour {

    private static AnimationTermite instance;

    public static AnimationTermite Instance{
        get{
            if (instance == null) {
                instance = FindObjectOfType<AnimationTermite>();
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
    public bool idle;
    public bool mayDie = false;
    private bool mayDrop = true;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player").transform;

        if(idle==false){
        navSpeed = GetComponent<NavMeshAgent>().speed;
        }

        attackRange = 6.6f;
	}

	void Update () {
        if (idle == false) {
        EnemyFollow();
        }
	}

	void EnemyFollow (){
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);
            if (distance < range){
                animator.SetBool("TermSolWalk", true);
                animator.SetBool("TermSolWalkStop", false);
                FollowPlayer();
            }
            else {
                animator.SetBool("TermSolWalk", false);
                animator.SetBool("TermSolWalkStop", true);
                Roam();
                resetBool = true;
            }

            if (distance < attackRange){
                animator.SetBool("TermSolAttackStart", true);
                animator.SetBool("TermSolWalk", false);
                agent.Stop();
                transform.LookAt(player);
                //GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(GetComponent<SoundSource>().playerDamageTaking);
            }
            else {
                animator.SetBool("TermSolAttackStart", false);
                animator.SetBool("TermSolWalk", true);
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
            mayDie = false;  
            GameObject.Find("Player").GetComponent<Experience>().currentExp += GameObject.Find("Player").GetComponent<Experience>().expGet;
            GameObject.Find("Spawn").GetComponent<SpawnEnemy>().spawned -= 1;
            animator.SetBool("TermSolDeath", true);
            animator.SetBool("TermSolWalk", false);
            animator.SetBool("TermSolWalkStop", true);
            transform.Find("Termiet soldier model").GetComponent<GivePlayerDamage>().damageForPlayer = 0;
            moveSpeed = 0;
            navSpeed = 0;
            if (player.GetComponent<Quests>().quest1[3] == true || player.GetComponent<Quests>().quest1[4] == true){
                player.GetComponent<Quests>().currentObjective ++;
                player.GetComponent<Quests>().currentObjectiveText ++;
                player.GetComponent<Quests>().LoopForBool();
                if(player.GetComponent<Quests>().quest1[5] == true){
                    player.GetComponent<Quests>().quest1[3] = false;
                    player.GetComponent<Quests>().quest1[4] = false;
                }
            }
            Destroy(gameObject, 1f);
            GameObject.Instantiate(dropRandomItem).transform.position = transform.position;
            mayDrop = false;
            if(GameObject.Find("Player").GetComponent<Quests>().quest1[8] == true){
                GameObject.Find("Player").GetComponent<Quests>().quest1_1 += 1;
            }   
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
