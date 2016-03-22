using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to the waterspatter in level 2 and level 3, it is spawned when the player or other enemies falls into the sewer water or waterfall
/// </summary>
public class WaterSplash : MonoBehaviour
{
	public GameObject waterSplash;
	public float VerticalOffset = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			Instantiate (waterSplash, other.transform.position + new Vector3 (0, VerticalOffset, 0), Quaternion.identity);
//						other.GetComponent<PlayerControl> ().DeathByDrawning ();
		}

		if (other.tag == "SpecialGround") {
			Instantiate (waterSplash, other.transform.position + new Vector3 (0, VerticalOffset, 0), Quaternion.identity);
			other.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);
			StartCoroutine ("Kill", other.gameObject);
		}
		if (other.tag == "Enemy") {
			Instantiate (waterSplash, other.transform.position + new Vector3 (0, VerticalOffset, 0), Quaternion.identity);
			other.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);
			Destroy (other.gameObject);
		}
				
	}

	IEnumerator Kill (GameObject o)
	{
		yield return new WaitForSeconds (0.8f);
		Destroy (o);
	}
}
