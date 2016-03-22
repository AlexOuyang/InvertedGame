using UnityEngine;
using System.Collections;
/// <summary>
/// Object tilt is attached to objects to achieve auto rotation based on the rotation of the ground
/// </summary>
public class ObjectTilt : MonoBehaviour
{
		public float rayCastLength = 1f;
		public float damping = 5f;
		// Update is called once per frame
		void FixedUpdate ()
		{
				// auto rotate the player according the the normal of the ground
				RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, rayCastLength, 1 << LayerMask.NameToLayer ("Ground"));
				Debug.DrawLine (transform.position, transform.position - (Vector3)(Vector2.up * rayCastLength), Color.red);
		
				if (hit.collider != null && hit.collider.tag == "SpecialGround") {
						Vector3 axis = Vector3.Cross (-transform.up, -hit.normal);
						if (axis != Vector3.zero && axis.z < 0.7) {
								float angle = Mathf.Atan2 (Vector3.Magnitude (axis), Vector3.Dot (-transform.up, -hit.normal));
								transform.RotateAround (axis, angle * Time.deltaTime * damping);
								//Debug.Log ("rotate");
						}
				} else {
						transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.identity, Time.time * damping);
				}

		}
}
