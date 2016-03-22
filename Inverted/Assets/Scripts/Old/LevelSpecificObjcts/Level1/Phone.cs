using UnityEngine;
using System.Collections;

public class Phone : MonoBehaviour {
	public GameObject PhoneCloseUp;
	
	private Transform player;
	private float time = 0;
	private float timeInterval = 2;
	private bool fistTimeTouch = true;


	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		
		PhoneCloseUp.SetActive(false);

	}
	
	// Update is called once per frame
	void Update(){
		
	}


	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			if(fistTimeTouch) {
				time = Time.time;
				fistTimeTouch = false;
			}
			if(Time.time - timeInterval > time) {
				// Also freeze the player
				player.GetComponent<PlayerControl>().freeze = true;
				
				PhoneCloseUp.SetActive(true);

				this.gameObject.SetActive(false);
			   	time = Time.time;
				fistTimeTouch = true;
			}

		}
	}

	void OnCollisionExit (Collision coll) 
	{
		if (coll.gameObject.tag == "Player") {
			time = Time.time;
			fistTimeTouch = true;
		}
	}

}
