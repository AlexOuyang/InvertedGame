using UnityEngine;
using System.Collections;

/// <summary>
/// Spikes is attached to the spike objects in level 3 to cause the death of the player
/// </summary>
public class Spikes : MonoBehaviour
{
		public Collider2D supportCollider;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter2D (Collider2D coll)
		{
				if (coll.tag == "Player") {
						coll.GetComponent<PlayerControl> ().Death (300);
						supportCollider.enabled = false;
				}
				
		}
}
