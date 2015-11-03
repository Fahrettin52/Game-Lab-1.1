using UnityEngine;
using System.Collections;

public class GiveDamage : MonoBehaviour {

	public float nextSec;
	public float secRate;
	public RaycastHit rayHit;
	public float rayDistance;
	public int damagePunch;

	void Update () {
		GivePunch ();
	}

	public void GivePunch (){
		if(Input.GetButtonDown("Fire1")){
			Debug.DrawRay(transform.position, transform.forward, Color.green, rayDistance);
			if(Time.time > nextSec){
				nextSec = Time.time + secRate;
				if(Physics.Raycast(transform.position, transform.forward, out rayHit, rayDistance)){
					if(rayHit.transform.tag == "Enemy"){
						print("Enemy");
						rayHit.transform.gameObject.GetComponent<AnimationTermite>().DropDead(damagePunch);
					}
				}
			}
		}
	}
}
