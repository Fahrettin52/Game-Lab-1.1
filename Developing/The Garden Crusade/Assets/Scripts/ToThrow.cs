using UnityEngine;
using System.Collections;



public class ToThrow : MonoBehaviour {
    public int rayDis;
    public RaycastHit rayHit;
    public bool canThrow;
    public GameObject throwPrefab;
    

    void Start() {
    }

    void Update() {


        if (canThrow == true && Input.GetButtonDown("G"))
        {
            Instantiate(throwPrefab, transform.position + new Vector3(0, 1.3f, 1), Quaternion.identity);
            canThrow = false;
        }
        if (Input.GetButtonDown("F")) {
            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDis))
            {
                if (rayHit.transform.tag == "mayThrow")
                {
                    Destroy(GameObject.FindWithTag("mayThrow"));
                    canThrow = true;
                }
            }
        }

    }
        void Throw () {
        
        }
    }
