using UnityEngine;
using System.Collections;
/// <summary>
/// This script is attached to sewer piples in Level 3 Maze scene so that when the player goes into one pipe and come out of another pipe.
/// </summary>
public class Teleportation : MonoBehaviour {
	public Transform destination;
	
	//Vector3 teleportOffset = new Vector2(5,0);
	
	void Start() {

	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if(other != null && other.tag.Equals("Player")){
			other.transform.position = destination.position;
		}
		
	}
}
