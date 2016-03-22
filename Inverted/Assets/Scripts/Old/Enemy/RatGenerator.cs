using UnityEngine;
using System.Collections;

// Used to generate Rats in Level 2
public class RatGenerator : MonoBehaviour
{
		public GameObject enemy;
		public bool isLeft = true;
		public float generatingPeriod = 20f;           // How long the generation of enemies is going to last    
		public float generationProbility = 1f;            // The lower the probility, the lower the generation rate
		
		private float generateNextEnemy = 0;           // Keeps track of the generation rate
		private float generationRate = 2;
		private bool startGenerating = false;
		private bool isTriggered = false;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Time.time > generateNextEnemy && startGenerating) {
						generationRate = Random.Range (0, 1f);
						generateNextEnemy = Time.time + generationRate;

						// Randomly respawn a prey
						if (generationRate <= generationProbility) {
								GameObject e = Instantiate (enemy, transform.position, Quaternion.identity) as GameObject;
								GenerateEnemy (e);
						}
				}
		}

		void GenerateEnemy (GameObject enemy)
		{
				// Change the sorting layer to be random as well
				enemy.GetComponent<Renderer> ().sortingOrder = (int)Random.Range (23, 25);
				if(enemy.GetComponent<Rat>() !=null)  enemy.GetComponent<Rat> ().speed = Random.Range (4.0f, 7.0f);
				// Flip the preys and change the order layer if they are from the right generator
				if (!isLeft) {
						enemy.GetComponent<Rat> ().Flip ();
						enemy.GetComponent<Renderer> ().sortingOrder = (int)Random.Range (13, 15);
			
				}
		}

		void OnTriggerStay2D (Collider2D other)
		{
				if (other.tag == "Player" && !isTriggered) {
						startGenerating = true;
						isTriggered = true;
						StartCoroutine (StopGeneration ());
				}
		}

		IEnumerator StopGeneration ()
		{
				yield return new WaitForSeconds (generatingPeriod);
				startGenerating = false;
				Destroy (this.gameObject);
		}

}
