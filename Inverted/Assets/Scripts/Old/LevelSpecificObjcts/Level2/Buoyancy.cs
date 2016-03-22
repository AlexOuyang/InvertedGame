using UnityEngine;
using System.Collections;
/// <summary>
/// Attached to objects that falls into the sewer to simulate water floating effect
/// </summary>
public class Buoyancy : MonoBehaviour
{
		public float UpwardForce = 75f;
		public GameObject SplashParticles;
		public bool canFloat = true;                  // If canFloat is false, then the object would sink with in a certain amount of time
		public 	float timeTillSink = 5f;              // The amount of time for the object float on water
		public float horizontalVelocity = 5f;
		private bool isInWater = false;
		private bool splash = true;
		private bool sink = false;
		private Rigidbody2D rigid;
		private Color originalColor;

		void Start ()
		{
				rigid = GetComponent<Rigidbody2D> ();
				originalColor = GetComponent<SpriteRenderer> ().color;

				if (!canFloat) {
						StartCoroutine (Sink ());
						StartCoroutine (Kill ());
				}
		}

		// While touching Water's collider, exerting force to make the object flow
		void OnTriggerEnter2D (Collider2D other)
		{
				if (other.tag == "Water" && !sink) {
						isInWater = true;
						rigid.drag = 5f;
						rigid.angularDrag = 5f;
						rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
						if (splash) {
								Instantiate (SplashParticles, transform.position, Quaternion.identity);
								splash = false;
						}
				}
				
				
				if (other.name == "ObjectChangeColorDetector") {
						GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);

				}
		
		}

		void OnTriggerStay2D (Collider2D other)
		{
			
		}

		// Upon leaving Water's collider, add less force to make it sink
		void OnTriggerExit2D (Collider2D other)
		{
				if (other.tag == "Water" && !sink) {
						isInWater = false;
						rigid.drag = 0.5f;
						rigid.angularDrag = 0.5f;
						GetComponent<SpriteRenderer> ().color = originalColor;

				}
				// If the object touches Water's ColorChangeDetector, object color is changed to be underwater color

				if (other.name == "ObjectChangeColorDetector") {
						GetComponent<SpriteRenderer> ().color = originalColor;

				}

		}
	
		void FixedUpdate ()
		{
				if (isInWater && !sink) {
						Vector2 force = Vector2.up * UpwardForce;
						rigid.AddForce (force, ForceMode2D.Force);
						//Debug.Log (isInWater);
						Vector2 waterVelocity = Vector2.right * horizontalVelocity;
						rigid.AddForce (waterVelocity, ForceMode2D.Force);
				} else if (sink) {

				}
		}

		IEnumerator Sink ()
		{
				yield return new WaitForSeconds (timeTillSink);
				sink = true;
				rigid.drag = 0.5f;
				rigid.angularDrag = 0.5f;
				GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.2f);
		}

		IEnumerator Kill ()
		{
				yield return new WaitForSeconds (timeTillSink + 1);
				Destroy (this.gameObject);
		}
}
