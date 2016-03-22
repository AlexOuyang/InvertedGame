using UnityEngine;
using System.Collections;

/// <summary>
/// IgnoreCollision is used by objects to avoid collision with other objects, input objects wish to be ignored into collidersToIgnore[]
/// </summary>

public class IgnoreCollision: MonoBehaviour {
	public Collider2D[] collidersToIgnore;

	// Use this for initialization
	void Start () {
		foreach (Collider2D col in collidersToIgnore) {
			Physics2D.IgnoreCollision (col, GetComponent<Collider2D>());
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
