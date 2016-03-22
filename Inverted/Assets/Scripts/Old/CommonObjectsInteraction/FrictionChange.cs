using UnityEngine;
using System.Collections;
/// <summary>
/// Friction change is attached to objects that interacts with the player, such as boxes. It changes friction after continously being 
/// in contact with the player for a short amount of time so that the player is able to push the objects
/// </summary>

public class FrictionChange : MonoBehaviour
{
		public Collider2D collider;
		public float staticFriction = 1.5f;
		public float kineticFriction = 0.3f;
		private float timeOnTouchBeforeChangingFriction = 20f;
		private float time;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{


		}

		void OnCollisionStay2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Player") {
						time++;
						if (time > timeOnTouchBeforeChangingFriction && collider.sharedMaterial.friction != kineticFriction) {
								// Change from static friction to kinetic friction, but needs to reenable collider to do so
								collider.enabled = false;
								collider.sharedMaterial.friction = kineticFriction;
								collider.enabled = true;
				
						}

				} 
				Debug.Log ("Enter: " + time);
		}

		void OnCollisionExit2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Player" && collider.sharedMaterial.friction != staticFriction) {
						time = 0;
						collider.enabled = false;
						collider.sharedMaterial.friction = staticFriction;
						collider.enabled = true;
			
				} 
				Debug.Log ("Exit: " + time);
		}

}
