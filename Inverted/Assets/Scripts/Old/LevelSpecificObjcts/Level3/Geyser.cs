using UnityEngine;
using System.Collections;

// Attached to the Geysers in Level 3 spider boss scene
public class Geyser : MonoBehaviour
{
		public float fireSpeed = 2000;
		public float fireRate = 3f;
		private float nextFire = 0f;
		private bool isActivated = false;
		private Animator anim;

		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
				nextFire = Random.Range (1, 10);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Time.time > nextFire) {
						nextFire = Time.time + fireRate;
						isActivated = true;
						FireStream ();
				} 
		}

		void FireStream ()
		{
				anim.SetTrigger ("isActivated");
				StartCoroutine ("Deactivate");
		}

		IEnumerator Deactivate ()
		{
				yield return new WaitForSeconds (0.2f);
				isActivated = false;
		}

		void OnTriggerStay2D (Collider2D other)
		{
		
				if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") {
						if (isActivated) {
								other.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * fireSpeed);
						}
				}
							
		}

}
