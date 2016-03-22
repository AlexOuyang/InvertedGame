using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to the moon object in level 3 to follow the player
/// </summary>
public class MoonFollow : MonoBehaviour
{
	public float damping = 8.0f;

	private Transform camera;

	// Use this for initialization
	void Start ()
	{
		camera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (camera != null) {
			Vector3 endPos = new Vector3 (camera.position.x, transform.position.y, transform.position.z);
			transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * damping);
		}
	}
}
