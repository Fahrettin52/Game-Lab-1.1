using UnityEngine;
using System.Collections;

public class DestroySounds : MonoBehaviour {


	void Update () {
        if (!GetComponent<AudioSource>().isPlaying) {
            Destroy(gameObject);
        }
	}
}
