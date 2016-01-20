using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour{

    public float forwardSpeed;
    public int backwardSpeed;
    public int crouchSpeed;
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
    public bool secondMode;
    public float backRotSpeed;
    public float eulerangle = 90;
    public bool rotate;
 
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

    void Update () {
    	if(mayMove == true){
    		Rotate2D ();
    	}
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
	           	
            if (Input.GetAxis("Vertical") > 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, rayDistance)) {
                    transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
                    if (!GetComponent<AudioSource>().isPlaying){
                        GetComponent<AudioSource>().PlayOneShot(GameObject.Find("SoundSource").GetComponent<SoundSource>().playerWalk);
                    }
                }
            }

            if (Input.GetAxis("Vertical") < 0  && secondMode == false) {
                forwardSpeed = backwardSpeed;
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
            sarah.GetComponent<Animator>().SetBool("MayCrouch", true);
            sarah.GetComponent<Animator>().SetTrigger("MayCrouchWalk 0");
            forwardSpeed = crouchSpeed;
        }
        else{
            GetComponent<CapsuleCollider>().height = 2.62f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 1.2f, 0);
            sarah.GetComponent<Animator>().SetBool("MayCrouch", false);
        }
    }

    public void Run() {
        if (Input.GetButton("Run")) {
            sarah.GetComponent<Animator>().SetBool("MayRun", true);
            forwardSpeed = runSpeed;
        } 
        else {
            forwardSpeed = normalSpeed;
            sarah.GetComponent<Animator>().SetBool("MayRun", false);
        }
    }

    public void Rotate2D () {

	 if(secondMode == false){
       		transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
            sarah.GetComponent<AnimationSara>().SarahTurn(Input.GetAxis("Horizontal"));
        }

       	if(secondMode == true){
           	if(Input.GetButtonDown("S")){
           		rotate =! rotate;
           		print(rotate);
           		if(rotate == true){
           			transform.eulerAngles = new Vector3(0, eulerangle, 0);
           		}
           		if(rotate == false){
           			transform.eulerAngles = new Vector3(0, -eulerangle, 0);
           		}
           	}
        }
	}
}


      

