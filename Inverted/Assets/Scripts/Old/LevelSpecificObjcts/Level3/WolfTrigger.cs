using UnityEngine;
using System.Collections;

/// <summary>
/// WolfTrigger is attached to the Trigger gameObject, a child object of Wolf enemy
/// it informs the camera to focus on the Wolf gameObject and disables the player temporarily
/// until the camera is back to focusing on the player
/// </summary>
public class WolfTrigger : MonoBehaviour
{
		public bool isTriggered = false;
		public float triggerDelay = 0.7f;
		public float waitBeforeFocusingOnPlayerOffset = 0.5f;
		public Transform wolf;
		private Transform camera;
		private float waitBeforeFocusingOnPlayer;
		private float cameraOffsetOnWolf;

		void Awake ()
		{
				wolf = transform.parent.transform;
				if (wolf == null)
						Debug.Log ("WolfTrigger can't find Wolf");
				camera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
				waitBeforeFocusingOnPlayer = wolf.GetComponent<WolfControl> ().waitBeforeChasing + waitBeforeFocusingOnPlayerOffset;
				cameraOffsetOnWolf = wolf.GetComponent<WolfControl> ().cameraOffset;
		}

		void Update ()
		{
				//Debug.Log(isTriggered);
		}

		void OnTriggerEnter2D (Collider2D coll)
		{
				//only can trigger once
				if (coll.gameObject.tag == "Player" && !isTriggered) {
			coll.GetComponent<PlayerControl> ().freeze = true;
						StartCoroutine (TriggerDelay ());
				}
		}

		IEnumerator TriggerDelay ()
		{
				yield return new WaitForSeconds (triggerDelay);
				isTriggered = true;
//				camera.GetComponent<CameraScript> ().FocusOnEnemy (wolf, cameraOffsetOnWolf, waitBeforeFocusingOnPlayer);
		}
}
