using UnityEngine;
using System.Collections;

public class InteractionWithEnvironment : MonoBehaviour {

	public float rayDis;
	public float forwardWhenJump;
	public float maxHeight;

	public int jumpsSpeed;
	private int moveSpeed = 5;

	private int moveToTarget;
	
	private Rigidbody rb;
	private RaycastHit rayHit;
	public GameObject wallText;
	public Transform target;
	public GameObject pasCollider;
	
	void Start () {

		rb = GetComponent<Rigidbody>();
		wallText = GameObject.Find("Wall Jump Text");
		wallText.SetActive(false);
		target = GameObject.Find("Environment Element/Transform").transform;
		pasCollider = GameObject.Find("Environment Element/PassageCollider");
		pasCollider.SetActive(false);
	}
	
	void Update (){

	}

	void FixedUpdate () {
		//Interaction ();
	}

	void Interaction () {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
			if(rayHit.transform.tag == "EnviroElement"){
				wallText.SetActive(true);
				if(Input.GetButtonDown("Fire1")){
					GetComponent<Movement>().enabled = false;
					rb.velocity =  new Vector3(0, jumpsSpeed * Time.deltaTime ,0);
				}	
			}
		}
		else{
			wallText.SetActive(false);
		}
		if(transform.position.y > maxHeight){
			transform.Translate(0, 0, forwardWhenJump * Time.deltaTime);
		}

		Vector3 down = transform.TransformDirection(Vector3.down);
		float actSpeed = moveSpeed * Time.deltaTime;
		if(Physics.Raycast(transform.position, down, out rayHit, rayDis)){
			if(rayHit.transform.tag == "EnviroElement" && moveToTarget == 0){
				transform.position = Vector3.MoveTowards (transform.position, target.position, actSpeed);
				GetComponent<Movement>().enabled = false;
				if(transform.position == target.position){
					moveToTarget += 1;
					GetComponent<Movement>().enabled = true;
				}
			}
		}

		if(Physics.Raycast(transform.position, down, out rayHit, rayDis)){
			if(rayHit.transform.tag == "Grond"){
				GetComponent<Movement>().enabled = true;
				moveToTarget = 0;
			}
		}

		if(moveToTarget == 1){
			pasCollider.SetActive(true);
			if(Physics.Raycast(transform.position, fwd, out rayHit, rayDis)){
				if(rayHit.transform.tag == "PassageCollider"){
					wallText.SetActive(true);
					if(Input.GetButtonDown("Fire1")){
						moveToTarget += 1;
						pasCollider.SetActive(false);
						rb.velocity =  new Vector3(0, jumpsSpeed * Time.deltaTime ,0);
						GetComponent<Movement>().enabled = false;
					}
				}
			}
		}
	}
}
