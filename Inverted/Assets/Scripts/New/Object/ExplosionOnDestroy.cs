using UnityEngine;
using System.Collections;
/// <summary>
/// Explosion on destroy is attached to the explosion object so the game object is destroyed after the explosion animation
/// </summary>

public class ExplosionOnDestroy : MonoBehaviour
{
		public float lifeSpan = 1.0f;
		// Use this for initialization
		void Start ()
		{
				StartCoroutine (DestroySelf ());
		}
	
		IEnumerator DestroySelf ()
		{
				yield return new WaitForSeconds (lifeSpan);
				Transform.Destroy (transform.gameObject);
		}
}
