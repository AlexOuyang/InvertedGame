using UnityEngine;
using System.Collections;

//unofficial
public class CameraScriptTest : MonoBehaviour 
{
	public float distanceFromPlayer = 10.0f;
	public float cameraOffset = 1.0f;
	public float damping = 5.0f;

	public float CameraDown = -3.1f;
	public float CameraUp = -2.8f;
	public float CameraLeft = -8.0f;
	public float CameraRight = 15.0f;

	private Transform player;
	
	// camera zoom
	public float zoomedOrthographicSize = 1.6f;
	public float normalOrthographicSize = 1.3f;
	public float smooth = 5.0f;
	
	private bool zoomed = false;

	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;

	}
	void Start () {
	}
	
	void Update () {
		Vector3 endPos = player.TransformPoint(0, cameraOffset, -distanceFromPlayer);
		if (endPos.y < CameraDown) {
			endPos.y = CameraDown;
		} else if (endPos.y > CameraUp) {
			endPos.y = CameraUp;
		}
		if (endPos.x < CameraLeft) {
			endPos.x = CameraLeft;
		} else if (endPos.x > CameraRight) {
			endPos.x = CameraRight;
		}
		transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * damping);


		if (Input.GetKeyDown(KeyCode.Z)) {
			zoomed = !zoomed;
		}
		
		if (zoomed){
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize,zoomedOrthographicSize,Time.deltaTime*smooth);
			
		}else{
			GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize,normalOrthographicSize,Time.deltaTime*smooth);
		}
	
	}
	
}
