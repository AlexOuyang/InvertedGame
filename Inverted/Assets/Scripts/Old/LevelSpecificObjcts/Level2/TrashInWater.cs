using UnityEngine;
using System.Collections;
/// <summary>
/// Trash in water is attached to each trash object in water so that when the trash flows from point A to point B, it would restart flowing from point A
/// </summary>
public class TrashInWater : MonoBehaviour {
	private float leftSideRestriction;
	private float rightSideRestriction;
	private float speed;
	// Use this for initialization
	void Start () {
		leftSideRestriction = GameObject.Find("LeftSide").transform.position.x;
		rightSideRestriction = GameObject.Find("RightSide").transform.position.x;
		speed = Random.Range(0.5f,3.5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector2.right * Time.deltaTime * speed);

		if(transform.position.x > rightSideRestriction) {
			transform.position = new Vector3(leftSideRestriction, transform.position.y, transform.position.z);
		}
	}
}
