using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to objects that kills player such as spikes
/// </summary>
public class Obstacle : MonoBehaviour {


	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<PlayerControl> ().Death (300);
		}
		
	}
}
