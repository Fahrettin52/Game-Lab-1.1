using UnityEngine;
using System.Collections;

public class GivePlayerDamage : MonoBehaviour
{

    public int damageForPlayer;
    public GameObject sarah;
    public bool hitCooldown;
    public float cooldown;

    void Start()
    {
        hitCooldown = false;
    }

    void OnCollisionStay(Collision col)
    {
        if(col.transform.tag == "Player" && hitCooldown == false)
        {
            StartCoroutine(CoolDownDmgTaken());
        }
    }

    IEnumerator CoolDownDmgTaken()
    {
        sarah.GetComponent<PlayerScript>().currentHealth -= damageForPlayer;
        sarah.GetComponent<PlayerScript>().HandleHealth();
        hitCooldown = true;
        yield return new WaitForSeconds(cooldown);
        hitCooldown = false;
    }
}
