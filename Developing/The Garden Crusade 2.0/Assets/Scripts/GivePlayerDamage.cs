using UnityEngine;
using System.Collections;

public class GivePlayerDamage : MonoBehaviour
{

    public int damageForPlayer;
    public GameObject sarah;
    public bool hitCooldown;
    public float cooldown;
    public Animator animator;
    public int rockDamage;
    public bool continueAttack;

    void Start()
    {
        hitCooldown = false;
        sarah = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "Player") {
            animator.SetBool("TermSolAttackStart", true);
        }
        if (col.transform.tag == "mayThrow") {
            animator.SetBool("TermSolAttackStart", true);
            GetComponentInParent <AnimationTermite>().DropDead(rockDamage);
            Invoke("AggresiveStateCooldown", 1f);
        }
    }

    void OnTriggerExit(Collider col) {
        //animator.SetBool("TermSolAttackStart", true);
        //Invoke("AggresiveStateCooldown", 1f);
        if (col.transform.tag == "Player") {
            continueAttack = false;
            animator.SetBool("TermSolAttackStart", false);
            animator.SetBool("MayAttackPlayer", false);
        }
    }

    void OnTriggerStay(Collider col) {
        if (Input.GetButtonDown("Fire1")){
            continueAttack = true;
        }
        if (col.transform.tag == "Player" && hitCooldown == false && continueAttack == true) {
            animator.SetBool("MayAttackPlayer", true);
            StartCoroutine(CoolDownDmgTaken());
        }
    }

    IEnumerator CoolDownDmgTaken() {
        sarah.GetComponent<Stamina>().RageBar();
        sarah.GetComponent<PlayerScript>().currentHealth -= damageForPlayer;
        sarah.GetComponent<PlayerScript>().HandleHealth();
        hitCooldown = true;
        yield return new WaitForSeconds(cooldown);
        hitCooldown = false;
    }

    void AggresiveStateCooldown() {
        animator.SetBool("TermSolAttackStart", false);
    }
}
