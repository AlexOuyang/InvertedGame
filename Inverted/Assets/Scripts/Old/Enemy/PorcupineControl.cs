using UnityEngine;
using System.Collections;

public class PorcupineControl : MonoBehaviour
{
		public float speed = 1.0f;
		public float range = 2.0f;
		public float startPositionX;
		
		private Animator anim;					  // Reference to the player's animator component.
		private float direction = 1.0f;
		private float flipRate = 1f;
		private float nextFlip = 0;
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
				if ((transform.position.x > startPositionX + range || transform.position.x < startPositionX - range) && Time.time > nextFlip) { 
						Flip ();
						nextFlip = Time.time + flipRate;
				} 
				transform.position = new Vector2 (transform.position.x + (speed * direction) * Time.deltaTime, transform.position.y); 

		}

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Player" && !alreadyKilledHamster) {
						coll.gameObject.GetComponent<PlayerControl> ().Death ();
						alreadyKilledHamster = true;
				}
		}

		void Flip ()
		{
				direction *= -1;
				Vector3 scale = transform.localScale;
				scale.x *= -1;
				transform.localScale = scale;
				//touchPlayer = false;
		}
	
}
