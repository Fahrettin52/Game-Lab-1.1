using UnityEngine;
using System.Collections;

public class GivePlayerDamage : MonoBehaviour
{

    public int damageForPlayer;
    public GameObject sarah;
    public bool hitCooldown;
    public float cooldown;
    public Animator animator;

    void Start()
    {
        hitCooldown = false;
        sarah = GameObject.Find("Player");
    }

    void OnCollisionEnter(Collision col) {
        animator.SetBool("TermSolAttackStart",true);
    }

    void OnCollisionExit(Collision col) {
        animator.SetBool("TermSolAttackStart", false);
        animator.SetBool("MayAttackPlayer", false);
    }

    void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Player" && hitCooldown == false && Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("MayAttackPlayer", true);
            StartCoroutine(CoolDownDmgTaken());
        }
    }

    IEnumerator CoolDownDmgTaken()
    {
        sarah.GetComponent<Stamina>().RageBar();
        sarah.GetComponent<PlayerScript>().currentHealth -= damageForPlayer;
        sarah.GetComponent<PlayerScript>().HandleHealth();
        hitCooldown = true;
        yield return new WaitForSeconds(cooldown);
        hitCooldown = false;
    }
}
