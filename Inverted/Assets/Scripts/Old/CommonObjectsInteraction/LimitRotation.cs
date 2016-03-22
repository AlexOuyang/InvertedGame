using UnityEngine;
using System.Collections;

/// <summary>
/// Limit rotation is attached to objects that use physics in order to prevent objects from fliping over due to physical forces
/// </summary>

public class LimitRotation : MonoBehaviour {

	public float zRotationLimit = 0;
	public float rotationSpeed = 10f;

	void Update () {
		float rotation = transform.rotation.eulerAngles.z;
		if(rotation >180) rotation = 360 - rotation;

		//Debug.Log(rotation);
		if(Mathf.Abs(rotation) > zRotationLimit){
			//Debug.Log("Tilting");
			//newAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, zRotationLimit);
			//transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, zRotationLimit);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
		}
	}
}
