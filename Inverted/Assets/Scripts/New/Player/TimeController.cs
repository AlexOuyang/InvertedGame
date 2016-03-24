using UnityEngine;
using System.Collections;
/// <summary>
/// Time controller controlls the time.
/// </summary>
public class TimeController : MonoBehaviour
{
	private bool slowDownTime = false;

	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.T)) {
			slowDownTime = !slowDownTime;
		}
		Time.timeScale = (slowDownTime) ? 0.3f : 1f;
	}
	
}