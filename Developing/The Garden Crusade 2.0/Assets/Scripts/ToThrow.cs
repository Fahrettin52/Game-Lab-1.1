using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToThrow : MonoBehaviour {
    public float rayDis;
    public RaycastHit rayHit;
    public bool canThrow = false;
    public GameObject throwPrefab;
    public GameObject throwInfo;
    public Rigidbody rb;
    public float throwSpeed;
    public string[] changeText;
    public GameObject trowPos;
    public float mayPickUp;
    public int currentHold;
    public int maxHold;
    public Animator sarah;
    public GameObject soundToOpen;
    public float lifeTimeSound;

    void Start() {
        //throwInfo.SetActive(false);
        rb = GameObject.Find("Kruimels").GetComponent<Rigidbody>();
        mayPickUp = 1f;
    }

    void Update() {

            mayPickUp -= Time.deltaTime;
        if (mayPickUp <= 0){
            mayPickUp = 0;
        }
            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDis) ||
               (Physics.Raycast(transform.position + new Vector3(0, 0, 0), transform.forward, out rayHit, rayDis) ||
               (Physics.Raycast(transform.position + new Vector3(0, 2.6f, 0), transform.forward, out rayHit, rayDis)))) {
                if (rayHit.transform.tag == "mayThrow" && mayPickUp == 0 && currentHold < 10){
                    currentHold++;
                    Destroy(rayHit.transform.gameObject);
                    mayPickUp = 1;
                    throwInfo.GetComponent<Text>().text = "Rocks: " + currentHold.ToString("F0");
                    canThrow = true;
                }
            }

        if (Input.GetButtonDown("F") && currentHold <=maxHold && currentHold >0 && canThrow == true) {
            sarah.GetComponent<Animator>().SetTrigger("MayThrow");
            currentHold--;
            Invoke("ThrowRock", 0.25f);
            canThrow = false;
        }
    }

    public void ThrowRock() {
        StartCoroutine(ThrowCooldown());
    }

    IEnumerator ThrowCooldown() {
        GameObject tempPrefab = Instantiate(throwPrefab, trowPos.transform.position + transform.forward, Quaternion.identity) as GameObject;
        ThrowRockSound();
        tempPrefab.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0) + transform.forward * throwSpeed;
        mayPickUp = 1;
        throwInfo.GetComponent<Text>().text = "Rocks: " + currentHold.ToString("F0");
        yield return new WaitForSeconds(1f);
        canThrow = true;
        
    }
    public void ThrowRockSound() {
        Instantiate(soundToOpen, throwPrefab.transform.position, transform.rotation);
        Destroy(GameObject.Find("ThrowingSound(Clone)"), lifeTimeSound);
    }
}

