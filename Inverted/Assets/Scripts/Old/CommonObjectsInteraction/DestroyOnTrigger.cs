using UnityEngine;
using System.Collections;

/// <summary>
/// Destroy on trigger is attached to the edge objects, when player falls out of the screen they will be killed by edge objects
/// </summary>

public class DestroyOnTrigger : MonoBehaviour
{

		void OnTriggerEnter2D (Collider2D other)
		{
				if (other.tag == "Player") {
						other.GetComponent<PlayerControl> ().Death ();
				}
		}

		void OnCollisionEnter2D (Collision2D other)
		{
				if (other.gameObject.tag == "Player") {
						other.gameObject.GetComponent<PlayerControl> ().Death ();
				}
		}
}
