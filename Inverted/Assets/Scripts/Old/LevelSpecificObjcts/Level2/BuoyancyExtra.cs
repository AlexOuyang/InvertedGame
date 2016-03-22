using UnityEngine;
using System.Collections;
/// <summary>
/// This script is used to change object's color when they are underwater, such as boxes in Level 2
/// </summary>
public class BuoyancyExtra : MonoBehaviour {
	private GameObject objectUnderWater;
	private Color originalColor;    


	void Start () {
		objectUnderWater = this.transform.parent.gameObject;
		originalColor = GetComponent<SpriteRenderer> ().color;

	}

	void OnTriggerStay2D (Collider2D other)
	{
			objectUnderWater.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);
		
	}
	
	void OnTriggerExit2D (Collider2D other)
	{
		if (other.name == "Water") {
			objectUnderWater.GetComponent<SpriteRenderer> ().color = originalColor;
			
		}
	}

}
