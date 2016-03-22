using UnityEngine;
using System.Collections;
/// <summary>
/// Platform is attached to each platform objects to achieve the scatter and closing in animation effect at the begining of the game
/// </summary>
public class Platform : MonoBehaviour
{
		public bool kill = false;
		private float Distance = 3f;
		private float TimeScale = 2.0f;
		private Animator animator;
		private Vector3 target;
		private bool backToOriginalPosition = false;
		private Vector3 delta;


		// Use this for initialization
		void Start ()
		{

				target = transform.position;
				Distance += Mathf.Abs (Mathf.Sin (transform.position.x / 5f)) * 19f;
				TimeScale += Mathf.Abs (Mathf.Sin (transform.position.x / 5f)) * 3.3f;
				transform.position = transform.position - transform.rotation * Vector3.up * Distance;

				animator = GetComponent<Animator> ();
				if (animator != null) {
						//animator.playbackTime = Random.Range(0f, 0.4f);
						animator.speed = Random.Range (1f, 1.3f);
				}
				
	
				StartCoroutine (BackToOriginalPosition ());
		}

		void Update ()
		{
				if (!backToOriginalPosition)
						transform.position = Vector3.Lerp (transform.position, target, Time.deltaTime * TimeScale);

		}

		void OnTriggerEnter2D (Collider2D other)
		{
				if (!kill)
						return;

				if (other.tag == "Player") {
						GameObject.Find ("Player").GetComponent<PlayerControl> ().Death ();
				}

		}

		IEnumerator BackToOriginalPosition ()
		{
				yield return new WaitForSeconds (1.5f);
				backToOriginalPosition = true;
		}

}
