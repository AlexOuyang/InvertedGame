using UnityEngine;
using System.Collections;
/// <summary>
/// Rock fall is attached to the waterfall rock object so that it will fall soon after comes in contact with the player
/// </summary>
public class RockFall : MonoBehaviour
{
		public GameObject rockFall;
		public float rockFallWaitTime = 0.4f;
		public bool isSpecialRock = false;
		private Rigidbody2D _rigid;
		private bool onlyTouchRockOnce = true;
		// Use this for initialization
		void Start ()
		{
				_rigid = GetComponent<Rigidbody2D> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Player") {
						if (onlyTouchRockOnce) {
								StartCoroutine (RockFallDown ());
								onlyTouchRockOnce = false;
						}
				}

		}

		IEnumerator RockFallDown ()
		{
				yield return new WaitForSeconds (rockFallWaitTime);
				_rigid.isKinematic = false;
				// First disable the water Splatter 
				transform.GetChild (1).gameObject.SetActive (false);
				transform.GetChild (0).gameObject.SetActive (false);


				if (!isSpecialRock) {
						GetComponent<Collider2D> ().enabled = false;
				} else {
						GameObject rock = Instantiate (rockFall, transform.position, Quaternion.Euler (0, 0, -20f)) as GameObject;
						Destroy (this.gameObject);
				}
		

		}
}
