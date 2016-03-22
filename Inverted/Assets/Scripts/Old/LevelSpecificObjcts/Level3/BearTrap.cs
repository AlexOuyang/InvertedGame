using UnityEngine;
using System.Collections;

/// <summary>
/// Bear trap can only used to kill enemy or the player once. It is used in level 3
/// </summary>

public class BearTrap : MonoBehaviour
{
		public Animator bearTrapTopLayerAnim;
		public Collider2D[] wolfColliders;                    // Used to ignore wolf's collider that's not a trigger and only detect collision through isTrigger
		public Collider2D bearTrapBottomCollider;
		private Animator anim;
		private bool isActivated;

		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
				// Ignore collision between the wolf and the bear trap
				foreach (Collider2D wolfCollider in wolfColliders) {
						Physics2D.IgnoreCollision (wolfCollider, bearTrapBottomCollider);
				}

		}

		void OnTriggerEnter2D (Collider2D coll)
		{
				// The Enemy here is wolf because its the only moving enemy in level 3
				if (coll.gameObject.tag == "Enemy" && !isActivated) {
						coll.gameObject.GetComponent<WolfControl> ().isDead = true;
						coll.gameObject.GetComponent<WolfControl> ().Death ();

						isActivated = true;
						Activate (0);
				}

				if (coll.gameObject.tag == "Player" && !isActivated) {
						coll.gameObject.GetComponent<PlayerControl> ().Death (300);

						isActivated = true;
						Activate (1000);
				}
		}

		void Activate (int bearTrapTopSortingLayerOrder)
		{
				// Play snap animation and audio, disable colliders
				anim.SetTrigger ("isActivated");
				bearTrapTopLayerAnim.SetTrigger ("isActivated");
				bearTrapTopLayerAnim.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = bearTrapTopSortingLayerOrder;
			
				Destroy (GetComponent<Rigidbody2D> ());
				Destroy (bearTrapBottomCollider);

		}
}
