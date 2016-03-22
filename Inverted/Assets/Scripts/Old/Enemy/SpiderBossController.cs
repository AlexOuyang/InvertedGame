using UnityEngine;
using System.Collections;

public class SpiderBossController : MonoBehaviour {

	private Animator anim;	// Reference to the player's animator component.
	private SpiderBossShooter shooterScript;


	void Awake(){
		anim = GetComponent<Animator>();
	}

	void Start () {
		shooterScript = transform.FindChild("Shooter").GetComponent<SpiderBossShooter>();

		StartCoroutine(FireProjectile());
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y)){
			shooterScript.IsActive = !shooterScript.IsActive;
		}
	}

	IEnumerator FireProjectile(){
		yield return new WaitForSeconds(4);
		anim.SetTrigger("Fire");
		shooterScript.IsActive = true;

	}
}
