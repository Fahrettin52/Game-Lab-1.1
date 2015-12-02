using UnityEngine;
using System.Collections;



public class ToThrow : MonoBehaviour {
    public float rayDis;
    public RaycastHit rayHit;
    public bool canThrow;
    public GameObject throwPrefab;
    public GameObject throwInfo;
    public Rigidbody rb;
    public float throwSpeed;
    public string[] changeText;

    void Start() {
        throwInfo.SetActive(false);
        rb = GameObject.Find("testThrow").GetComponent<Rigidbody>();
    }

    void Update() {
        if (Input.GetButtonDown("F")) {
            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDis) ||
               (Physics.Raycast(transform.position + new Vector3(0, 0, 0), transform.forward, out rayHit, rayDis) ||
               (Physics.Raycast(transform.position + new Vector3(0, 2.6f, 0), transform.forward, out rayHit, rayDis)))) {
                if (rayHit.transform.tag == "mayThrow") {
                    Destroy(GameObject.FindWithTag("mayThrow"));
                    throwInfo.SetActive(true);
                    canThrow = true;
                }
            }
        }
        if (canThrow == true && Input.GetButtonDown("G")) {
            Instantiate(throwPrefab, transform.position + new Vector3(0, 1.3f, 0) + transform.forward, Quaternion.identity);
            rb = GameObject.Find("testThrow(Clone)").GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, 0) + transform.forward * throwSpeed;
            throwInfo.SetActive(false);
            canThrow = false;
        }
    }
}

