using UnityEngine;
using System.Collections;

public class SummoningInsects : MonoBehaviour {

	public GameObject fireFly;

	public bool switchLight;

	public int  newIntensity;
	public int  oldIntensity;

	public float oldRange;
	public float newRange;
	public float moveSpeed;

	public Transform newLightPos;
	public Transform oldLightPos;


	void Start () {

	}
	
	void Update () {
		print(switchLight);
		MaxIntensity ();

		if(Application.loadedLevel == 4){
			fireFly.SetActive(true);
		}
		else{
			fireFly.SetActive(false);
		}
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
			fireFly.transform.position = Vector3.MoveTowards(fireFly.transform.position, newLightPos.position, moveSpeed * Time.deltaTime);
		}
		else{
			fireFly.GetComponent<Light>().intensity = oldIntensity;
			fireFly.GetComponent<Light>().range = oldRange;
			fireFly.transform.position = oldLightPos.position;
		}
	}
}
