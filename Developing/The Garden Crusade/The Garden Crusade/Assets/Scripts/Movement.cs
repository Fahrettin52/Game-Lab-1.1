using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour{

    public float forwardSpeed;
    public float rotateSpeed;
    public int mouseSpeed;
    public int rayDistance;
    public int strafeSpeed;

    void Update(){
        MoveAndRotate();
    }

    void MoveAndRotate(){

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        /*if (!Physics.Raycast(transform.position, forward, 10))
        {
            float speed = forwardSpeed * Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }*/

        //Achteruit
        if (Input.GetAxis("Vertical") > 0){
            if (Physics.Raycast(transform.position, transform.forward, rayDistance)){
                print("Hit");
            }
            else{
                transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            }
        }

        if (Input.GetAxis("Vertical") < 0){
            if (Physics.Raycast(transform.position, -transform.forward, rayDistance)){
                print("Hit");
            }
            else{
                transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            }
        }

        if (Input.GetButton("E")) {
            print("Input");
            if (Physics.Raycast (transform.position, -transform.right, rayDistance)) {
                print ("Hit");
            } 
        }
        else{
             transform.Translate (Vector3.left * strafeSpeed * Time.deltaTime);
            }

        if (Input.GetButton("Q")){
            if (Physics.Raycast (transform.position, transform.right, rayDistance)) {
                print ("Hit");
            } 
        }
        else {
                transform.Translate (Vector3.right * strafeSpeed  * Time.deltaTime);
        }
    }
}


      

