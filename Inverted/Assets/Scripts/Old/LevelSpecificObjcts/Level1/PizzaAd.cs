using UnityEngine;
using System.Collections;

// This script is attached to the pizza add object, once the pizza add
// is clicked, the phone number is displayed
public class PizzaAd : MonoBehaviour {
	public GameObject pizzaCloseUp;

	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;

		pizzaCloseUp.GetComponent<SpriteRenderer>().enabled = false;
		pizzaCloseUp.GetComponent<Collider2D>().enabled = false;

	}
	
	// Update is called once per frame
	void Update(){

	}

	void OnMouseDown()
	{
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;

		// Also freeze the player
		player.GetComponent<PlayerControl>().freeze = true;

		pizzaCloseUp.GetComponent<SpriteRenderer>().enabled = true;
		pizzaCloseUp.GetComponent<Collider2D>().enabled = true;
	}
}
