using UnityEngine;
using System.Collections;

/// <summary>
/// Arrow remains plays the remains animation.
/// </summary>
public class ArrowRemains : MonoBehaviour
{
	private float _direction = 1f;
	private float _lifeSpan = 5.0f;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine ("destroySelf");	
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

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Platform") {
		}
	}

	IEnumerator destroySelf ()
	{
		yield return new WaitForSeconds (_lifeSpan);
		Transform.Destroy (transform.gameObject);
	}

	/**
	 * Used to flip the arrow to the correct direction. direction is either 1 or -1
	 */
	public void Flip (float direction)
	{
		this._direction = direction;
		Vector3 theScale = transform.localScale;
		theScale.x = Mathf.Abs (theScale.x) * direction;
		transform.localScale = theScale;
	}
}
