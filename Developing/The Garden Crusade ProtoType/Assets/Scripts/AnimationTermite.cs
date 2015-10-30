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

    void Start () {
		player = GameObject.Find("Player").transform;
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
            }

            if (distance < attackRange)
            {
                animator.SetBool("TermSolAttackStart", true);
                animator.SetBool("TermSolWalk", false);
            }
            else
            {
                animator.SetBool("TermSolAttackStart", false);
                animator.SetBool("TermSolWalk", true);
            }
        }
	}

	void FollowPlayer (){
		transform.LookAt(player);
    	transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
