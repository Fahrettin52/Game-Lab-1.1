using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour{

    public float forwardSpeed;
    public int normalSpeed;
    public int runSpeed;
    public float rotateSpeed;
    public int mouseSpeed;
    public float rayDistance;
    public int strafeSpeed;
    public GameObject sarah;
    public bool mayJump;
    public Vector3 jumpSpeed;
    public float grondDis;
    private Rigidbody rb;
    public int dubbleJump;
    public int maxJump;


    void Start() {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        MoveAndRotate();
        Crouch();
        Run();
    }

    void FixedUpdate (){
    	jump();
    }

    public void jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, grondDis))
        {
            mayJump = true;
            //rb.useGravity = false;
            dubbleJump = 0;
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (Input.GetButtonDown("Jump") && dubbleJump < maxJump)
        {
            sarah.GetComponent<AnimationSara>().mayJump();
            rb.velocity += jumpSpeed;
            dubbleJump ++;
        }
        if (dubbleJump > 0)
        {
            jumpSpeed.y = 0;
        }
        if (jumpSpeed.y == 0)
        {
            jumpSpeed.y = 5;
        }
    }
    // 		 	if(Physics.Raycast(transform.position,Vector3.down,grondDis)){
    //		mayJump = true;
    //		GetComponent.<Rigidbody>().useGravity = false;
    //		GetComponent.<Rigidbody>().velocity.y = 0;
    //		dubbleJump = 0;
    //	}
    //	else{
    //		GetComponent.<Rigidbody>().useGravity = true;
    //	}
    //	if(Input.GetButtonDown("Jump")&& dubbleJump<maxJump){
    //		GetComponent.<Rigidbody>().velocity += jumpSpeed;
    //		dubbleJump +=1;
    //	}
    //	if(dubbleJump >1){
    //		jumpSpeed.y = 0;
    //	}
    //	if(jumpSpeed.y == 0){
    //		jumpSpeed.y = 5;
    //	}
    //}

    void MoveAndRotate(){

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
//        Vector3 forward = transform.TransformDirection(Vector3.forward);

        /*if (!Physics.Raycast(transform.position, forward, 10))
        {
            float speed = forwardSpeed * Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }*/

        //Achteruit
        if (Input.GetAxis("Vertical") > 0){
            sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
            if (Physics.Raycast(transform.position, transform.forward, rayDistance)){
            }
            else{
                transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            }
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            sarah.GetComponent<AnimationSara>().animator.SetBool("cancelRun", true);
        }

        if (Input.GetAxis("Vertical") < 0){
            sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
            if (Physics.Raycast(transform.position, -transform.forward, rayDistance)){
            }
            else{
                transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            }
        }

        if (Input.GetButton("E")) {
            if (Physics.Raycast (transform.position, -transform.right, rayDistance)) {
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

    public void Crouch()
    {
        if (Input.GetButton("Crouch"))
        {
            GetComponent<CapsuleCollider>().height = 0.5f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 0.4f, 0);
        }
        else
        {
            GetComponent<CapsuleCollider>().height = 2.62f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 1.14f, 0);
        }
    }
    public void Run() {
        if (Input.GetButton("Run")) {
            forwardSpeed = runSpeed;
        } else {
            forwardSpeed = normalSpeed;
        }
    }
}


      

