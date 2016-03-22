using UnityEngine;
using System.Collections;

public class WolfControl: MonoBehaviour
{
	public float AttackRange = 2.5f;
	//the attack range for playing the attack animation
	public float ChasingSpeed = 20.0f;
	//speed of the wolf
	public bool isDead = false;
	//is the Wolf enemy alive
	public float waitBeforeChasing = 3.0f;
	//wait and play the holw and idel animation before chasing the player
	public float waitAfterDeathThenDisappear = 4.0f;
	//wait for seconds after death then destroy the body
	public float cameraOffset = 3.0f;
	// set up the camera focus when the camera transforms to focus on the wolf
	public Transform trigger;
	//the childObject Trigger

	public BoxCollider2D boxCollider2D;
	//disabling the collider after the wolf dies so the palyer can walk by it

	private float playerDistanceFromEnemy;
	//the distance between the player and the wolf
	private Transform Target;
	//Target is the Player
	private float direction = 1.0f;
	//used for fliping the direction of the wolf
	private PlayerControl player;
	//get the PlayerControl script from the Player
	private Vector2 TargetDirection = Vector2.zero;
	private Animator anim;
	// Reference to the player's animator component.
	private bool facingRight = true;
	// For determining which way the player is currently facing.
	private int howl = 0;
	// Reset holw = 0 to trigger howling animatino once
	private AudioSource audio;
	private bool alreadyKilledHamster = false;
	private Vector2 chasingDirection = new Vector2 (-15, 3);

	void Awake ()
	{
		Target = GameObject.FindGameObjectWithTag ("Player").transform;
		player = Target.GetComponent<PlayerControl> ();
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();

	}

	void Update ()
	{
		anim.SetFloat ("Speed", Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x));
				
		//Debug.Log (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x));
	}

	void FixedUpdate ()
	{
		if (!isDead) {
			if (!player.died) {
				if (trigger.GetComponent<WolfTrigger> ().isTriggered) {
					//only howl once
					howl++;
					if (howl == 1) {
						anim.SetTrigger ("Howl");
						StartCoroutine (PlayHowlAudio ());
					}
					StartCoroutine (WaitThenChase ());
				}
			} else {
				//if player is dead, eat the player and howl
				//Eating();
//								GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
//								GetComponent<Rigidbody2D> ().mass = 100;

				anim.SetTrigger ("Howl");

			}
		} else {
//						StartCoroutine (Death ());
		}
		
	}
		
	//Chase() helper function
	IEnumerator WaitThenChase ()
	{
		yield return new WaitForSeconds (waitBeforeChasing);
		ChaseAndAttack ();
	}

	void ChaseAndAttack ()
	{
		//chasing the player
		playerDistanceFromEnemy = Vector2.Distance (Target.transform.position, transform.position);
		float Xdifference = Target.position.x - transform.position.x;
		float Ydifference = Target.position.y - transform.position.y;
		TargetDirection = new Vector2 (Xdifference, Ydifference);
		GetComponent<Rigidbody2D> ().AddForce (TargetDirection.normalized * ChasingSpeed);
				
//				GetComponent<Rigidbody2D> ().AddForce (chasingDirection);

		//transform.Translate(TargetDirection * Time.deltaTime * ChasingSpeed);


		if (playerDistanceFromEnemy < AttackRange) {
			//Attack ();
		}
		
		if (TargetDirection.x < 0 && !facingRight) {
			//Debug.Log("Looking Right");
			Flip ();
		}
		if (TargetDirection.x > 0 && facingRight) {
			//Debug.Log("Looking Left");
			Flip ();
		}
	}

	void Attack ()
	{
		anim.SetTrigger ("Attack");
		//		if (Time.time > NextAttack){
		//			shootingAudio.Play();
		//			Target.SendMessage("ApplyDamage", Damage);
		//			Debug.Log("The Enemy Has Attacked");//testing purpose
		//			NextAttack = Time.time + AttackRate;
		//			
		//			animation.Play("ReaperAttack",PlayMode.StopAll);
		//		}
	}

	void Eating ()
	{
		//eating animation after player dies
	}

	public void Death ()
	{
		anim.SetTrigger ("Death");
		GetComponent<Rigidbody2D> ().isKinematic = true;
		boxCollider2D.enabled = false;

		StartCoroutine ("DestroyDeadWolf");

	}

	IEnumerator DestroyDeadWolf ()
	{
		yield return new WaitForSeconds (waitAfterDeathThenDisappear);
		Transform.Destroy (transform.gameObject);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Obstacles") {
			isDead = true;
			Death ();

		}
		if (coll.gameObject.tag == "Player" && !alreadyKilledHamster) {
			coll.gameObject.GetComponent<PlayerControl> ().Death ();
			alreadyKilledHamster = true;
		}

	}

	void Flip ()
	{
		if (!isDead) {
			direction *= -1;
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	IEnumerator PlayHowlAudio ()
	{
		yield return new WaitForSeconds (0.5f);
		audio.Play ();
	}

	
}
