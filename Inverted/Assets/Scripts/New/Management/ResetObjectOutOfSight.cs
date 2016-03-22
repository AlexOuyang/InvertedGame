using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to an object so that once it flies out of range, it is rest to where it was set before
/// </summary>
public class ResetObjectOutOfSight : MonoBehaviour
{
		public GameObject targetObject;               // Selected object to be reset once out of sight
		
		private string name;
		private Vector3 originalPosition;
		private Quaternion originalQuaternion;

		void Start ()
		{
				originalPosition = targetObject.transform.position;
				originalQuaternion = targetObject.transform.rotation;
				name = targetObject.name;
				
				RegenerateTarget ();
		}
	
		void Update ()
		{
	
		}

		// Reset their position once they are out of sight
		void OnTriggerExit2D (Collider2D coll)
		{
				Debug.Log (coll.gameObject.name);
				if (coll.gameObject.name == targetObject.name) {
						RegenerateTarget ();
//						targetObject.transform.position = originalPosition;
//						targetObject.transform.rotation = originalQuaternion;
//						targetObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
						//Destroy (coll.gameObject);
				}
		}

		void RegenerateTarget ()
		{
				GameObject copy = Instantiate (targetObject, originalPosition, originalQuaternion) as GameObject;
				Destroy (targetObject);
				copy.name = name;
				targetObject = copy;
	
		}
}
