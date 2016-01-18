using UnityEngine;
using System.Collections;

public class SummoningInsects : MonoBehaviour {

	public GameObject fireFly;

	public bool switchLight;

	public int  newIntensity;
	public int  oldIntensity;

	public float oldRange;
	public float newRange;

	void Start () {

	}
	
	void Update () {
		print(switchLight);
		MaxIntensity ();
	}

	void OnTriggerEnter (Collider col){
		if(col.transform.tag == "FireFlyTrigger"){
			 switchLight =! switchLight;
		}
	}

	void MaxIntensity () {
		if(switchLight == true){
			fireFly.GetComponent<Light>().intensity = newIntensity;
			fireFly.GetComponent<Light>().range = newRange;
		}
		else{
			fireFly.GetComponent<Light>().intensity = oldIntensity;
			fireFly.GetComponent<Light>().range = oldRange;
		}
	}
}
