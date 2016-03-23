using UnityEngine;
using System.Collections;

public class ArrowShooting : MonoBehaviour
{
	public float maxScale = 1000.0f;
	public float speed = 100.0f;

	public float direction = 1f;
	private float lifeSpan = 5.0f;


	private Vector3 originalPos;
	private float originalScale;
	private float endScale;

	// Use this for initialization
	void Start ()
	{
		originalPos = transform.position;
		originalScale = transform.localScale.x;
		endScale = originalScale;

		StartCoroutine("destroySelf");
	
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Vector3 scale = transform.localScale;
//		scale.x = Mathf.MoveTowards (scale.x, endScale, Time.deltaTime * speed);
//		Debug.Log (scale.x);
//		transform.position = originalPos + new Vector3 ((scale.x / 2.0f + originalScale / 2.0f), 0f, 0f);
//		if (Input.GetKeyDown (KeyCode.S)) {
//			endScale = maxScale;
//		} else if (Input.GetKeyDown (KeyCode.R)) {
//			endScale = originalScale;
//		}

//		if (Input.GetKey (KeyCode.S)) {
//		transform.Translate (new Vector3 (direction, 0, 0) * Time.deltaTime * speed);
//		}
	
	}


	IEnumerator destroySelf ()
	{
		yield return new WaitForSeconds (lifeSpan);
		Transform.Destroy (transform.gameObject);
	}

	/**
	 * Used to flip the arrow to the correct direction. direction is 1 or -1
	 */
	public void Flip (float direction)
	{
		this.direction = direction;
		Vector3 theScale = transform.localScale;
		theScale.x = Mathf.Abs(theScale.x) * direction;
		transform.localScale = theScale;
	}
}
