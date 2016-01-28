using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public GameObject player;

	void Start () {
	
	}
	
	void Update () {
        player = GameObject.Find("Player");
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
