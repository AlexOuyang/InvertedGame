using UnityEngine;
using System.Collections;

/// <summary>
/// Water generator is attached to the WaterGenerator gameObject which is a child object of SewerOpeningGenerator
/// It is used to generate water pouring out of the sewer effect to kill player
/// </summary>
public class WaterGenerator : MonoBehaviour
{
		public GameObject waterPouring;
		public bool isLeft = true;
		public float waterLastingPeriod = 1f;
		private float generateNextEnemy = 0;           // Keeps track of the generation rate
		private float generationRate = 3.5f;
		private bool startGenerating = false;
		
		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Time.time > generateNextEnemy && startGenerating) {
						generateNextEnemy = Time.time + generationRate;
						GameObject o = Instantiate (waterPouring, transform.position, Quaternion.identity) as GameObject;
						StartCoroutine ("StopWaterGeneration", o);
				}
		}
	
		void OnTriggerStay2D (Collider2D other)
		{
				if (other.tag == "Player") {
						startGenerating = true;
				}
		}
	
		void OnTriggerExit2D (Collider2D other)
		{
				if (other.tag == "Player") {
						startGenerating = false;
				}
		}

		IEnumerator StopWaterGeneration (GameObject water)
		{
				yield return new WaitForSeconds (waterLastingPeriod);
				Destroy (water.gameObject);
		}

}
