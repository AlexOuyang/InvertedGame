using UnityEngine;
using System.Collections;
/// <summary>
/// Trash generator is attached to the TrashGenerator gameObject which is a child object of SewerOpeningGenerator
//  It is used to generate trash out of the sewer opening so that the player can jump on them
/// </summary>
public class TrashGenerator : MonoBehaviour
{
		public GameObject trash;
		public bool isLeft = true;
		public float generationProbility = 1f;            // The lower the probility, the lower the generation rate
	
		private float generateNextEnemy = 0;           // Keeps track of the generation rate
		private float generationRate = 2;
		private bool startGenerating = false;

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Time.time > generateNextEnemy && startGenerating) {
						generationRate = Random.Range (0, 10f);
						generateNextEnemy = Time.time + generationRate;
			
						if (generationRate <= generationProbility) {
								GameObject o = Instantiate (trash, transform.position, Quaternion.identity) as GameObject;
								o.GetComponent<Renderer> ().sortingOrder = (int)Random.Range (23, 25);
						}
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

}
