using UnityEngine;
using System.Collections;
/// <summary>
/// This script is attached to the trash objects at the begining of the level 2 so that the player won't touch the trash before the trash touch the sewer water
/// This is done to avoid complication between trash and the player
/// </summary>
public class TrashNoColliderBeforeHittingTheWater : MonoBehaviour {

	private bool haveEnteredWater = false;
	private Vector3 originalLocalScale;
	// Use this for initialization
	void Start () {
		originalLocalScale = transform.localScale;
		this.GetComponent<Collider2D>().isTrigger = true;
		transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// While touching Water's collider, exerting force to make the object flow
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Water" && !haveEnteredWater) {
			this.GetComponent<Collider2D>().isTrigger = false;
			transform.localScale = originalLocalScale;
			haveEnteredWater = true;
		}
	}
}
