using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
    public float maxVert;
    public float minVert;
    public float maxHor;
    public float minHor;
    public GameObject maxVertObj;
    public GameObject minVertObj;
    public GameObject maxHorObj;
    public GameObject minHorObj;
    public float timeToSpawn=5;
    public GameObject enemyPrefab;
    public int spawnMax;
    public int spawned;

    // Use this for initialization
    void Start () {
        maxVert = maxVertObj.transform.position.z;
        minVert = minVertObj.transform.position.z;
        maxHor = maxHorObj.transform.position.x;
        minHor = minHorObj.transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        timeToSpawn -= 1 * Time.deltaTime;
        if(timeToSpawn<=0 && spawned<spawnMax) {
            Instantiate(enemyPrefab, new Vector3(Random.Range(maxHor, minHor), transform.position.y, Random.Range(maxVert, minVert)), transform.rotation);
            timeToSpawn = 5;
        }
	}
}
