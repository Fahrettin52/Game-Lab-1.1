using UnityEngine;
using System.Collections;

public class GivePlayerDamage : MonoBehaviour {

	public int damageForPlayer;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
		if(col.transform.tag == "Player"){
			GameObject.Find("Player").GetComponent<PlayerScript>().currentHealth -= damageForPlayer;
		}
	}
}
