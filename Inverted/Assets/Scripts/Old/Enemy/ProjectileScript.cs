using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
		public Vector3 velocityVector;
		public GameObject explosion;
		public bool isBallistic;
		public float Drag; // in metres/s lost per second.
		public AudioSource explosionAudio;         

	
		// Use this for initialization
		void Start ()
		{
				StartCoroutine (DestroySelf ());
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Drag != 0)
						velocityVector += velocityVector * (-Drag * Time.deltaTime);
		
				if (isBallistic)
						velocityVector += Physics.gravity * Time.deltaTime;
		
				if (velocityVector == Vector3.zero)
						return;
				else
						transform.position += velocityVector * Time.deltaTime;

				Debug.DrawLine (transform.position, transform.position + velocityVector.normalized, Color.red);
		}

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Porcupine" || coll.gameObject.tag == "Ground") {
						Explode ();
				}
				if (coll.gameObject.tag == "Player") {
						Explode ();
						coll.gameObject.SendMessage ("Death");
				}
		}

		void Explode ()
		{
				explosionAudio.Play ();
				GameObject e = Instantiate (explosion, transform.position, Quaternion.identity) as GameObject;
				Transform.Destroy (transform.gameObject);
		}

		IEnumerator DestroySelf ()
		{
				yield return new WaitForSeconds (2);
				Transform.Destroy (transform.gameObject);
		}
	
}