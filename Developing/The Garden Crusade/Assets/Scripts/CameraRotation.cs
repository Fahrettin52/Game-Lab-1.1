using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	public Transform cameraTarget;
	private float x = 0.0f;
	private float y = 0.0f;
	public int mouseSpeedX;  	//3
	public int mouseSpeedY;		//2
	public float Limit1;		//-20
	public float Limit2;		//50
	public float maxViewDistance;		//15
	public float minViewDistance;		//3
//	private float distance = 3;
	private float desiredDistance;
	private float correcterDistance;
	public int zoomSpeed;		//30
	public int lerpSpeed;		//5
	public float cameraTargetHeight;		//1
	private float currentDistance;
	public bool activateReset = true;
    public CanvasGroup inventoryCanvas;  
    public CanvasGroup chestCanvas;
    public CanvasGroup characterCanvas;

    void Start () {
		Vector3 angles = transform.eulerAngles;
		x = angles.x;
		y = angles.y;
	}
	
	
	void LateUpdate () {
		if(Input.GetButton("Fire2") /*&& inventoryCanvas.alpha < 1 && chestCanvas.alpha < 1 && characterCanvas.alpha < 1*/)
        {
			x += Input.GetAxis("Mouse X") * mouseSpeedX;
			y += Input.GetAxis("Mouse Y") * -mouseSpeedY;
			activateReset = false;
		}
		else{
			activateReset = true;
		}

		if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetButton("Q") || Input.GetButton("E")){
			if(activateReset == true){
				float targetRotationAngle = cameraTarget.eulerAngles.y;
				float cameraRotationAngle = transform.eulerAngles.y;
				x = Mathf.LerpAngle(cameraRotationAngle, targetRotationAngle, lerpSpeed * Time.deltaTime);
			}
		}

		y = ClampAngle (y, Limit1, Limit2);

		Quaternion rotation = Quaternion.Euler (y, x, 0);

		desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed * Mathf.Abs(desiredDistance);
		desiredDistance = Mathf.Clamp(desiredDistance, minViewDistance, maxViewDistance);
		correcterDistance = desiredDistance;
		Vector3 position = cameraTarget.position - (rotation * Vector3.forward * desiredDistance);

		RaycastHit rayHit;
		Vector3 cameraTargetPosition = new Vector3(cameraTarget.position.x, cameraTarget.position.y + cameraTargetHeight, cameraTarget.position.z);
		bool isCorrected = false;
		if(Physics.Linecast(cameraTargetPosition, position, out rayHit)){
			position = rayHit.point;
			correcterDistance = Vector3.Distance(cameraTargetPosition, position);
			isCorrected = true;
		}

		currentDistance = !isCorrected || correcterDistance > currentDistance ? Mathf.Lerp(currentDistance, correcterDistance, Time.deltaTime * zoomSpeed) : correcterDistance;
		position = cameraTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3 (0, -cameraTargetHeight, 0));

		transform.rotation = rotation;
		transform.position = position;

	}

	private static float ClampAngle(float angle, float min, float max){
		if(angle < -360){
			angle += 360;
		}
		if(angle > 360){
			angle -= 360;
		}
		return Mathf.Clamp(angle, min , max);
	}
}
