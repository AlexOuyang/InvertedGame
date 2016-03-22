using UnityEngine;
using System.Collections;

public class Kid : MonoBehaviour
{
	public float speed = 1.0f;
	public float startPositionX;
	
	private Animator anim;					  // Reference to the player's animator component.
	private float direction = 1.0f;
	private bool alreadyKilledHamster = false;
	
	// Use this for initialization
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		
	}
	
	void Start ()
	{
		startPositionX = transform.position.x;
	}
	
	void Update ()
	{ 
//		if(!alreadyKilledHamster)
		transform.position = new Vector2 (transform.position.x + (speed * direction) * Time.deltaTime, transform.position.y); 
	}
	

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player" && !alreadyKilledHamster) {
			coll.gameObject.GetComponent<PlayerControl> ().Death ();
			alreadyKilledHamster = true;
		}	
	}

}
