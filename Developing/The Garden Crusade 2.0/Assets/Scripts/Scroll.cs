using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {

    public int scrollCounter;
    public Image [] skills;
    public GameObject player;
    public int skillCounter;

    public void OnTriggerEnter (Collider col) {
        if (col.tag == "Player") {
            ActiveSkill(scrollCounter);
            player.GetComponent<CastingBar>().skillActivate[skillCounter] = true;
            scrollCounter++;
            skillCounter++;
            Destroy(gameObject);
        }
        if (scrollCounter > skills.Length) {
            scrollCounter = 4;
        }
    }

    public void ActiveSkill(int counter) {
        skills[counter].enabled = true;
    }
}
