using UnityEngine;
using System.Collections;

public class GeneralDamage : MonoBehaviour {

    public int damageForPlayer;
    public GameObject sarah;
    public bool hitCooldown;
    public float cooldown;
    public int rockDamage;
    public bool continueAttack;

    void Update() {
        rockDamage = PlayerScript.Instance.agility;
    }

    void Start() {
        hitCooldown = false;
        sarah = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "Player") {
           
        }
        if (col.transform.tag == "mayThrow") {
            
        }
    }

    void OnTriggerExit(Collider col) {
        //animator.SetBool("TermSolAttackStart", true);
        //Invoke("AggresiveStateCooldown", 1f);
        if (col.transform.tag == "Player") {
            continueAttack = false;
           
        }
    }

    void OnTriggerStay(Collider col) {
        //if (Input.GetButtonDown("Fire1")) {
        //    continueAttack = true;
        //}
        if (col.transform.tag == "Player" && hitCooldown == false) {
            
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
        
    }
}
