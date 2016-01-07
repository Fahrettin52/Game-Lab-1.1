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
    public int totalSpawned;
    public int totalMax;
    //public GameObject walkTo;
    //public Vector3 resetWalkTo;

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
        if(timeToSpawn<=0 && spawned<spawnMax && totalSpawned<totalMax) {
            spawned++;
            totalSpawned++;
   
            enemyPrefab.GetComponent<AnimationWalkTermite>().maxHor = maxHor;
            enemyPrefab.GetComponent<AnimationWalkTermite>().minVert = minVert;
            enemyPrefab.GetComponent<AnimationWalkTermite>().maxVert = maxVert;
            Instantiate(enemyPrefab, new Vector3(Random.Range(maxHor, minHor), transform.position.y, Random.Range(maxVert, minVert)), transform.rotation);
            //Instantiate(walkTo, new Vector3(Random.Range(maxHor, minHor), transform.position.y, Random.Range(maxVert, minVert)), transform.rotation);

            timeToSpawn = 5;
        }
        if(spawned == spawnMax || totalSpawned == totalMax)
        {
            timeToSpawn = 5;
        }
        if(spawned <= 0)
        {
            spawned = 0;
        }
	}
}
