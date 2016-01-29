using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZwarteWeduwe : MonoBehaviour
{

    private static ZwarteWeduwe instance;

    public static ZwarteWeduwe Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ZwarteWeduwe>();
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
    public GameObject dropRandomItem;
    public Image fill;
    public NavMeshAgent agent;
    private bool resetBool;
    public float navSpeed;
    public bool idle;
    public bool mayDie = false;
    private bool mayDrop = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();

        if (idle == false)
        {
            navSpeed = GetComponent<NavMeshAgent>().speed;
        }
    }

    void Update()
    {
        if (idle == false)
        {
            EnemyFollow();
        }
    }

    void EnemyFollow()
    {
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);

            if (distance < range)
            {
                FollowPlayer();
                animator.SetBool("idleToWalk", true);
                animator.SetBool("walkToAttack", false);
            }
            else
            {
                resetBool = true;
                animator.SetBool("idleToWalk", false);
            }

            if (distance < attackRange)
            {
                animator.SetBool("idleToWalk", false);
                animator.SetBool("walkToAttack", true);
                animator.SetBool("idleToAttack", true);
                agent.Stop();
                transform.LookAt(player);
            }
            else
            {
                agent.Resume();
                animator.SetBool("walkToAttack", false);
                animator.SetBool("idleToAttack", false);
            }
        }
    }

    void FollowPlayer()
    {

        agent.SetDestination(player.transform.position);
        if (resetBool == true)
        {
            transform.LookAt(agent.destination);
            resetBool = false;
        }
    }

    public void DropDead(int damage)
    {
        livesEnemy -= damage;
        fill.fillAmount -= (damage / 2000f);
        if (livesEnemy < 1)
        {
            livesEnemy = 0;
        }
        if (livesEnemy == 0)
        {
            mayDie = true;
        }
        if (livesEnemy == 0 && mayDie == true && mayDrop == true)
        {
            mayDie = false;
            GameObject.Find("_Manager").GetComponent<ToSceneOne>().general = null;
            GameObject.Find("Player").GetComponent<Quests>().currentObjective += 1;
            GameObject.Find("Player").GetComponent<Quests>().currentObjectiveText += 1;
            GameObject.Find("Player").GetComponent<Quests>().LoopForBool();
            GameObject.Find("Player").GetComponent<Experience>().currentExp += GameObject.Find("Player").GetComponent<Experience>().expGet;
            //animator.SetTrigger("MayDie");
            GameObject.Find("Player").GetComponent<InteractionWithEnvironment>().weduweDood = true;
            Destroy(gameObject, 1f);
            GameObject.Instantiate(dropRandomItem).transform.position = transform.position;
            mayDrop = false;
        }
    }
}

