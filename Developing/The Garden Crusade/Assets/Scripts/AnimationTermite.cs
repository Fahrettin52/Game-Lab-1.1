using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimationTermite : MonoBehaviour {

    private static AnimationTermite instance;

    public static AnimationTermite Instance
    {
        get
        {
            if (instance == null)
            {
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
    private Vector3 resetPos;
    private bool resetBool;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
		player = GameObject.Find("Player").transform;
        resetPos = transform.position;
	}
	

	void Update () {
		EnemyFollow ();
	}

	void EnemyFollow (){
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);
            if (distance < range)
            {
                animator.SetBool("TermSolWalk", true);
                animator.SetBool("TermSolWalkStop", false);
                FollowPlayer();
            }
            else
            {
                animator.SetBool("TermSolWalk", false);
                animator.SetBool("TermSolWalkStop", true);
                
                agent.SetDestination(resetPos);
                resetBool = true;
            }

            if (distance < attackRange)
            {
                animator.SetBool("TermSolAttackStart", true);
                animator.SetBool("TermSolWalk", false);
                //agent.Stop();
            }
            else
            {
                animator.SetBool("TermSolAttackStart", false);
                animator.SetBool("TermSolWalk", true);
            }
        }
	}

	void FollowPlayer (){

        agent.SetDestination(player.transform.position);
        if (resetBool == true) {
            //agent.Resume();
            transform.LookAt(agent.destination);
            resetBool = false;
        }
    }

	public void DropDead (int damage){
        livesEnemy -= damage;
        fill.fillAmount -= (damage / 100f);
        if (livesEnemy < 1)
            {
                animator.SetBool("TermSolDeath", true);
                moveSpeed = 0;
                Destroy(gameObject, 2f);
                GameObject.Instantiate(dropRandomItem).transform.position = transform.position;
            Destroy(GameObject.Find("EnemyHP"));
            }
	}
}
