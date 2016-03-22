using UnityEngine;
using System.Collections;

/// <summary>
/// Camera script is attached to the main camera
/// </summary>
public class CameraScript : MonoBehaviour
{
	//player tracking and Camera position
	public float distanceFromPlayer = 10.0f;
	public float cameraOffset = 9.0f;
	public float damping = 5.0f;
	public float CamRestrictionRight = 3.0f;
	public float CamRestrictionLeft = 1000.0f;
	public float CamRestrictionUp = 100.0f;
	public float CamRestrictionDown = -100.0f;

	// camera zoom
	public float zoomedOrthographicSize = 1.5f;
	public float normalOrthographicSize = 2.0f;
	public float smooth = 5.0f;
	public bool zoomed = false;
	private bool moveToPlayerOnStart = true;

	private Transform player;


	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		this.transform.position = player.position;
	}

	void Update ()
	{
		
		if (Input.GetKeyDown (KeyCode.Z)) {
			zoomed = !zoomed;
		}
		
		if (zoomed) {
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, zoomedOrthographicSize, Time.deltaTime * smooth);
			
		} else {
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, normalOrthographicSize, Time.deltaTime * smooth);
		}
	}

	//camera movement needs to be executed in the LateUpdate()
	void LateUpdate ()
	{
		
		if (player != null) {
			Vector3 endPos = player.TransformPoint (0, cameraOffset, -distanceFromPlayer);
			if (endPos.x > CamRestrictionRight) {
				endPos.x = CamRestrictionRight;
			} else if (endPos.x < CamRestrictionLeft) {
				endPos.x = CamRestrictionLeft;
			}
			if (endPos.y > CamRestrictionUp) {
				endPos.y = CamRestrictionUp;
			} else if (endPos.y < CamRestrictionDown) {
				endPos.y = CamRestrictionDown;
			}
				
			transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * damping);

		} else {
			Debug.Log ("Camera can't find player");
		}
	
	}
	
}
