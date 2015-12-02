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

    void Update(){
        Crouch();
        Run();
    }

    void FixedUpdate (){
        MoveAndRotate();
        jump();
    }

    public void jump(){
        if (Physics.Raycast(transform.position, Vector3.down, grondDis)){
            mayJump = true;
            dubbleJump = 0;
        }
        else{
            mayJump = false;
        }

        if (Input.GetButton("Jump") && dubbleJump < maxJump) {
            sarah.GetComponent<AnimationSara>().mayJump();
            rb.velocity += jumpSpeed;
            dubbleJump ++;
        }
        if (dubbleJump > 0){
            jumpSpeed.y = 0;
        }
        if (jumpSpeed.y == 0){
            jumpSpeed.y = 5;
        }
    }

    void MoveAndRotate(){
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
    
        if (Input.GetAxis("Vertical") > 0){
            sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
            if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, rayDistance)){
                transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            }
        }

        if (Input.GetAxis("Vertical") < 0){
            sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
            if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), -transform.forward, rayDistance)){
                transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
            }
        }

        if (Input.GetAxis("Vertical") == 0) {
            sarah.GetComponent<AnimationSara>().animator.SetBool("cancelRun", true);
        }

        if (Input.GetButton("Q")) {
            if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), -transform.right, rayDistance)) {
                transform.Translate(Vector3.left * strafeSpeed * Time.deltaTime);
            }
        }

        if (Input.GetButton("E")) {
            if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.right, rayDistance)) {
                transform.Translate(Vector3.right * strafeSpeed * Time.deltaTime);
            }
        }
    }

    public void Crouch()
    {
        if (Input.GetButton("Crouch")) {
            GetComponent<CapsuleCollider>().height = 0.5f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 0.4f, 0);
        }
        else{
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


      

