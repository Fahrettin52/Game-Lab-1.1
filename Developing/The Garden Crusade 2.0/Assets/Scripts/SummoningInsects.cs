using UnityEngine;
using System.Collections;

public class SummoningInsects : MonoBehaviour {

	public GameObject fireFly;

	public bool switchLight;

	public int  newIntensity;
	public int  oldIntensity;

	void Start () {
		fireFly = GameObject.Find("FireFly Trigger");
	}
	
	void Update () {
	
	}

	void OnTriggerEnter (Collider col){
		if(col.transform.tag == "FireFlyTrigger"){
			 switchLight =! switchLight;
		}
	}

	void MaxIntensity () {
		if(switchLight == true){
			fireFly.GetComponent<Light>().intensity = newIntensity;
		}
		else{
			fireFly.GetComponent<Light>().intensity = oldIntensity;
		}
	}
}
