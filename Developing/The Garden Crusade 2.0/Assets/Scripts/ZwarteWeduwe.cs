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
    public MovieTexture movie;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;

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
            if (distance < 35)
            {
                //animator.SetBool("Threat", true);
                agent.Stop();
            }
            else
            {
                //animator.SetBool("Threat", false);
                agent.Resume();
            }

            if (distance < range)
            {
                FollowPlayer();
            }
            else
            {
                resetBool = true;
            }

            if (distance < attackRange)
            {
                //animator.SetTrigger("MayAttack");
                agent.Stop();
                transform.LookAt(player);
            }
            else
            {
                agent.Resume();
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
        fill.fillAmount -= (damage / 100f);
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
            Destroy(gameObject, 1f);
            GameObject.Instantiate(dropRandomItem).transform.position = transform.position;
            mayDrop = false;
        }
    }
    public void Play()
    {
        movie.Play();
    }
}

