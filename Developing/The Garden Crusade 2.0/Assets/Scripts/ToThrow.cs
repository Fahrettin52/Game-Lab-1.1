using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToThrow : MonoBehaviour {
    public float rayDis;
    public RaycastHit rayHit;
    public bool canThrow;
    public GameObject throwPrefab;
    public GameObject throwInfo;
    public Rigidbody rb;
    public float throwSpeed;
    public string[] changeText;
    public GameObject trowPos;
    public float mayPickUp;
    public int currentHold;
    public int maxHold;

    void Start() {
        throwInfo.SetActive(false);
        rb = GameObject.Find("Kruimels").GetComponent<Rigidbody>();
        mayPickUp = 3f;
    }

    void Update() {

            mayPickUp -= Time.deltaTime;
        if (mayPickUp <= 0){
            mayPickUp = 0;
        }
            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDis) ||
               (Physics.Raycast(transform.position + new Vector3(0, 0, 0), transform.forward, out rayHit, rayDis) ||
               (Physics.Raycast(transform.position + new Vector3(0, 2.6f, 0), transform.forward, out rayHit, rayDis)))) {
                if (rayHit.transform.tag == "mayThrow" && mayPickUp == 0){
                    currentHold++;
                    Destroy(rayHit.transform.gameObject);
                    throwInfo.SetActive(true);
                    //canThrow = true;
                    throwInfo.GetComponent<Text>().text = changeText[1];
                    mayPickUp = 1;
                }
            }
        if (Input.GetButtonDown("F") && currentHold <=maxHold && currentHold>0) {
            currentHold--;
            Instantiate(throwPrefab, trowPos.transform.position + transform.forward, Quaternion.identity);
            rb = throwPrefab.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, 0) + transform.forward * throwSpeed;
            if (currentHold == 0) {
                throwInfo.SetActive(false);
            }
            //canThrow = false;
            mayPickUp = 1;
        }
    }
}

