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
    public GameObject soundToOpenCrouch;
    public GameObject soundToOpenJump;
    public GameObject soundToOpenMove;
    public float lifeTimeSound;
    public bool maySoundMove;
    public int walkCooldown;
    public int runCooldown;
    public int walkTime;
    public int crouchCooldown;

    void Start() {
        grondDisJump = transform.localScale.y / 2f;
        rb = GetComponent<Rigidbody>();
        soundToOpenMove = GameObject.Find("PlayerWalkingSound");
        soundToOpenMove.SetActive(false);
        soundToOpenCrouch = GameObject.Find("PlayerCrouchingSound");
        soundToOpenCrouch.SetActive(false);
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
        MoveSound();
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
        if (Input.GetButtonDown("Jump") && mayJump == true) {
            JumpSound();
            rb.velocity = new Vector3(0, jumpSpeed, 0);

            }
        }

    void MoveAndRotate(bool mayMove){

        if (mayMove == true) {
	           	
            if (Input.GetAxis("Vertical") > 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, rayDistance)) {
                    transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
                    //MoveSound();
                    }
                }

            if (Input.GetAxis("Vertical") < 0  && secondMode == false) {
                forwardSpeed = backwardSpeed;
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), -transform.forward, rayDistance)){
                    transform.Translate(Vector3.forward * forwardSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
                    //MoveSound();
                    }
                }

            if (Input.GetAxis("Vertical") == 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                //sarah.GetComponent<AnimationSara>().animator.SetBool("cancelRun", true);
            }

            if (Input.GetAxis("Strave") == 0) {
                sarah.GetComponent<AnimationSara>().SarahRun(Input.GetAxis("Vertical"));
                //sarah.GetComponent<AnimationSara>().animator.SetBool("cancelRun", true);
            }

            if (Input.GetAxis("Strave") < 0) {
                sarah.GetComponent<AnimationSara>().Strave(Input.GetAxis("Strave"));
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), -transform.right, rayDistance)) {
                    transform.Translate(Vector3.right * forwardSpeed * Input.GetAxis("Strave") * Time.deltaTime);
                }
            }

            if (Input.GetAxis("Strave") > 0) {
                sarah.GetComponent<AnimationSara>().Strave(Input.GetAxis("Strave"));
                if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.right, rayDistance)) {
                    transform.Translate(Vector3.right * forwardSpeed * Input.GetAxis("Strave") * Time.deltaTime);
                }
            }
        }
    }

    public void Crouch()
    {
        if (Input.GetButton("Crouch")) {
            sarah.transform.localPosition = new Vector3(0, -0.41f, 0);
            GetComponent<CapsuleCollider>().direction = 2;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
            GetComponent<CapsuleCollider>().radius = 0.2f;
            sarah.GetComponent<Animator>().SetBool("MayCrouch", true);
            sarah.GetComponent<Animator>().SetTrigger("MayCrouchWalk 0");
            forwardSpeed = crouchSpeed;
            CrouchSound();
        }
        else{
            sarah.transform.localPosition = new Vector3(0, -0.082f, 0);
            GetComponent<CapsuleCollider>().direction = 1;
            GetComponent<CapsuleCollider>().height = 2.62f;
            GetComponent<CapsuleCollider>().radius = 0.45f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 1.2f, 0);
            sarah.GetComponent<Animator>().SetBool("MayCrouch", false);
            soundToOpenCrouch.SetActive(false);
        }
    }

    public void Run() {
        if (Input.GetButton("Run")) {
            sarah.GetComponent<Animator>().SetBool("MayRun", true);
            forwardSpeed = runSpeed;
            walkTime = runCooldown;
        } 
        else {
            forwardSpeed = normalSpeed;
            sarah.GetComponent<Animator>().SetBool("MayRun", false);
            walkTime = walkCooldown;
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
    public void CrouchSound() {
        if (soundToOpenCrouch.activeInHierarchy == false) {
            StartCoroutine(SoundCrouchStart());
        }
    }

    public void JumpSound() {
        Instantiate(soundToOpenJump, transform.position, transform.rotation);
        Destroy(GameObject.Find("PlayerJumpingSound(Clone)"), lifeTimeSound);
    }

    public void MoveSound() {
        if (Input.GetButton("Vertical") && soundToOpenMove.activeInHierarchy == false) {
            StartCoroutine(SoundMoveStart());
        }
    }

    IEnumerator SoundMoveStart() {
        soundToOpenMove.SetActive(true);
        soundToOpenMove.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(walkTime * Time.deltaTime);
        soundToOpenMove.SetActive(false);
    }

    IEnumerator SoundCrouchStart(){
        soundToOpenCrouch.SetActive(true);
        soundToOpenCrouch.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(crouchCooldown * Time.deltaTime);
    }
}


      

