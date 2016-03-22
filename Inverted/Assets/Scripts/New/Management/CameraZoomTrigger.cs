using UnityEngine;
using System.Collections;

public class CameraZoomTrigger : MonoBehaviour
{
		public float zoomedSize;
		public float offsetDifference = 0;
		private Transform camera;
		// Use this for initialization
		void Awake ()
		{
				camera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (camera == null) {
						Debug.Log ("no camera found");
				}
	
		}

		void OnTriggerEnter2D (Collider2D coll)
		{
				//only can trigger once
				if (coll.gameObject.tag == "Player") {
						camera.GetComponent<CameraScript> ().zoomed = true;
						camera.GetComponent<CameraScript> ().cameraOffset += offsetDifference;

						camera.GetComponent<CameraScript> ().zoomedOrthographicSize = zoomedSize;

				}
		}

		void OnTriggerExit2D (Collider2D coll)
		{
				//only can trigger once
				if (coll.gameObject.tag == "Player") {
						camera.GetComponent<CameraScript> ().zoomed = false;
						camera.GetComponent<CameraScript> ().cameraOffset -= offsetDifference;

				}
		}
}
