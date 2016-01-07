using UnityEngine;
using System.Collections;

public class DamageWithWalk : MonoBehaviour
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

    void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Player" && hitCooldown == false)
        {
            StartCoroutine(CoolDownDmgTaken());
        }
    }

    IEnumerator CoolDownDmgTaken()
    {
        sarah.GetComponent<Stamina>().RageBar();
        sarah.GetComponent<PlayerScript>().currentHealth -= damageForPlayer;
        sarah.GetComponent<PlayerScript>().HandleHealth();
        animator.SetBool("TermSolAttackStart", true);
        animator.SetBool("MayAttackPlayer", true);
        hitCooldown = true;
        yield return new WaitForSeconds(cooldown);
        hitCooldown = false;
    }
}
