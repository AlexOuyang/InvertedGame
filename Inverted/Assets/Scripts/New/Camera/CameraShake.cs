using UnityEngine;
using System.Collections;

// CameraShake is called by the ExplosiveSpider class after it is shot by the player
// the Shake() function disables the stalactites isKinematic function, allowing them to fall
public class CameraShake : MonoBehaviour{
	public float timeWaitToShake = 10f;

	private float shakeDecay;
	private float shakeIntensity;
	private Vector3 originPosition;
	private Quaternion originRotation;
	public GameObject[] shafts;

	void Start(){
		shafts = GameObject.FindGameObjectsWithTag("CameraShakeObject");
		StartCoroutine (ShakeCamera (timeWaitToShake));

	}
	
//	void OnGUI (){
//		if (GUI.Button (new Rect (20,40,80,20), "Shake")){
//			Shake ();
//		}
//	}
//	
	void Update (){
		if (shakeIntensity > 0){
			transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-shakeIntensity,shakeIntensity) * .2f,
				originRotation.y + Random.Range (-shakeIntensity,shakeIntensity) * .2f,
				originRotation.z, originRotation.w);
			shakeIntensity -= shakeDecay;

			if (shakeIntensity < 0.01f) {
				// Stop the lights
				foreach(GameObject s in shafts){
					s.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
		}
	}

	// Called by the ExplosiveSpider 
	// triggers the camera shaking mechanism
	public void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		shakeIntensity = .1f;
		shakeDecay = 0.0008f;


		// Shine shafts into the bedroom
		foreach(GameObject s in shafts){
			s.GetComponent<SpriteRenderer>().enabled = true;
		}

	}

	IEnumerator ShakeCamera(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Shake();
	}
	
}
