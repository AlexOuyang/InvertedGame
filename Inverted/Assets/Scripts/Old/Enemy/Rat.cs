using UnityEngine;
using System.Collections;

public class Rat : MonoBehaviour
{
		public float speed = 6.0f;
		public float startPositionX;
		public float direction = 1;
		private Animator anim;					  // Reference to the player's animator component.
		private bool alreadyKilledHamster = false;
	
		// Use this for initialization
		void Awake ()
		{
				anim = GetComponent<Animator> ();
		}
	
		void Start ()
		{
				startPositionX = transform.position.x;
		}
	
		void Update ()
		{ 
				if (!alreadyKilledHamster)
						transform.position = new Vector2 (transform.position.x + speed * direction * Time.deltaTime, transform.position.y); 
		}

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Player" && !alreadyKilledHamster) {
						coll.gameObject.GetComponent<PlayerControl> ().Death ();
						alreadyKilledHamster = true;

						// Play eating animatino
				}
		}

		public void Flip ()
		{
				direction *= -1;
				Vector3 scale = transform.localScale;
				scale.x *= -1;
				transform.localScale = scale;
				//touchPlayer = false;
		}
}
