using UnityEngine;
using System.Collections;


// This script is attached to the pizza add close up object, once the pizza add close up
// is clicked, the large pizza add disappear and the smaller one reappears on the fridge
public class PizzaAdCloseUp : MonoBehaviour {
	public GameObject pizzaAdSmall;

	private Transform player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;


	}
	
	// Update is called once per frame
	void Update(){
		
	}
	
	void OnMouseDown()
	{
		this.GetComponent<SpriteRenderer>().enabled = false;
		this.GetComponent<Collider2D>().enabled = false;

		//  Unfreeze the player
		player.GetComponent<PlayerControl>().freeze = false;


		pizzaAdSmall.GetComponent<SpriteRenderer>().enabled = true;
		pizzaAdSmall.GetComponent<Collider2D>().enabled = true;
	}
}
