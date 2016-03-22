using UnityEngine;
using System.Collections;

// Attached to the falling tree at level 3 - scene 3
public class FallingTree : MonoBehaviour {
	public GameObject rockOnTouchTreeTopFallingOff;
	public GameObject treeTop;

	private bool onlyPlayFallingAudioOnce = true;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.rotation.eulerAngles.z < 355 && transform.rotation.eulerAngles.z > 250) {
			Debug.Log("Play audio");
			if(onlyPlayFallingAudioOnce) {
				GetComponent<AudioSource>().Play();
				onlyPlayFallingAudioOnce = false;
			}
		}
		
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if(coll.gameObject.name == rockOnTouchTreeTopFallingOff.name) {
			treeTop.GetComponent<Rotate>().isRotating = true;            // Tree top rotating falling down animation
			treeTop.GetComponent<Rigidbody2D>().isKinematic = false;     // Allow tree top to fall
			Destroy(GetComponent<HingeJoint2D>());                       // Destroy tree rigidbody to allow it stay put
			Destroy(GetComponent<Rigidbody2D>());
			GetComponent<EdgeCollider2D>().isTrigger = false;            // Enable the bottom part of the tree collider
			GetComponent<BoxCollider2D>().sharedMaterial = null;         // delete material so that player can't charge while climbing up
		}

	}

}
