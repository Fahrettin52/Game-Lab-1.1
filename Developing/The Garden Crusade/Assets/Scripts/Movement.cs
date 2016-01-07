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
    public float jumpSpeed;
    public float grondDis;
    public float grondDisJump;
    private Rigidbody rb;
    public int dubbleJump;
    public int maxJump;
    public bool mayMove = true;

    void Start() {
        grondDisJump = transform.localScale.y / 2f;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate (){
        MoveAndRotate(mayMove);
        jump();
        Run();
        Crouch();
    }

    public void jump(){
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), -transform.up, grondDis) ||
           (Physics.Raycast(transform.position + new Vector3(-0.45f, 0, 0), -transform.up, grondDis) ||
           (Physics.Raycast(transform.position + new Vector3(0.45f, 0, 0), -transform.up, grondDis) ||
           (Physics.Raycast(transform.position + new Vector3(0, 0, 0.45f), -transform.up, grondDis) ||
           (Physics.Raycast(transform.position + new Vector3(0, 0, -0.45f), -transform.up, grondDis)))))){
            mayMove = true;
        } 
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), -transform.up, grondDisJump) ||
           (Physics.Raycast(transform.position + new Vector3(-0.45f, 0, 0), -transform.up, grondDisJump) ||
           (Physics.Raycast(transform.position + new Vector3(0.45f, 0, 0), -transform.up, grondDisJump) ||
           (Physics.Raycast(transform.position + new Vector3(0, 0, 0.45f), -transform.up, grondDisJump) ||
           (Physics.Raycast(transform.position + new Vector3(0, 0, -0.45f), -transform.up, grondDisJump)))))) {
            mayJump = true;
        } 
        else {
            mayJump = false;
        }
        sarah.GetComponent<AnimationSara>().mayJump1(mayJump);
        if (Input.GetButton("Jump") && mayJump == true) {
            rb.velocity = new Vector3(0, jumpSpeed, 0);
            if (!GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().PlayOneShot(GameObject.Find("SoundSource").GetComponent<SoundSource>().playerJump);
            }
        }
    }

    void MoveAndRotate(bool mayMove){
        if (mayMove == true) {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

            if (Input.GetAxis("Vertical") > 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, rayDistance)) {
                    transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
                   /* if (!GetComponent<AudioSource>().isPlaying){
                        GetComponent<AudioSource>().PlayOneShot(GameObject.Find("SoundSource").GetComponent<SoundSource>().playerWalk);
                    }*/
                }
            }

            if (Input.GetAxis("Vertical") < 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), -transform.forward, rayDistance)){
                    transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
                    if (!GetComponent<AudioSource>().isPlaying){
                        GetComponent<AudioSource>().PlayOneShot(GameObject.Find("SoundSource").GetComponent<SoundSource>().playerWalk);
                    }
                }
            }

            if (Input.GetAxis("Vertical") == 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
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
        } 
        else {
            forwardSpeed = normalSpeed;
        }
    }
}


      

