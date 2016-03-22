using UnityEngine;
using System.Collections;
/// <summary>
/// Explosive spider explodes short after touch the player, it can be used by the player to kill the spider boss by shooting them up using geysers in level 3
/// </summary>
public class ExplosiveSpider : MonoBehaviour
{

		public float speed = 1.0f;
		public float explosionCountDownTime = 3f;
		public GameObject explosion;
		public AudioSource explosionAudio;
		private float direction = 1;
		private Animator anim;					    // Reference to the player's animator component.
		private bool alreadyKilledHamster = false;
		private bool playerIsInExplosionRange = false;
	
		// Use this for initialization
		void Awake ()
		{
				anim = GetComponent<Animator> ();
		}
	
		void Start ()
		{
		}
	
		void Update ()
		{ 
				if (!alreadyKilledHamster) {
						transform.position = new Vector2 (transform.position.x + (speed * direction) * Time.deltaTime, transform.position.y); 
				}
		}

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Player" && !alreadyKilledHamster) {
						StartCoroutine (ExplosionDelay ());
						alreadyKilledHamster = true;
				}
		}
		
		void OnTriggerEnter2D (Collider2D other)
		{

				// Detect if player is in explosion Range
				if (other.gameObject.tag == "Player") {
						playerIsInExplosionRange = true;
						
				}
				
				if (other.gameObject.name == "Ceiling") {
						Explode ();

				}
		
		}

		void OnTriggerExit2D (Collider2D other)
		{
				if (other.gameObject.tag == "Player") {
						playerIsInExplosionRange = false;
			
				}

		}

		IEnumerator ExplosionDelay ()
		{
				yield return new WaitForSeconds (explosionCountDownTime);
				Explode ();

		}

		void Explode ()
		{
				explosionAudio.Play ();
				Instantiate (explosion, transform.position, Quaternion.identity);
				// Notify the player to die
				if (playerIsInExplosionRange) {
						PlayerControl.player.Death ();
				}
				Transform.Destroy (transform.gameObject);
		}

		
	
}
