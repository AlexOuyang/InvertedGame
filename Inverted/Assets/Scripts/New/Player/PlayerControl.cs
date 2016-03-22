﻿using UnityEngine;
using System.Collections;

// This script is attached to the player
public class PlayerControl : MonoBehaviour
{
	// Static reference to the playerControl script
	public static PlayerControl player;


	/*========= Player Attributes =========*/

	// In cut scene player have no spawn animation
	public bool spawnAnimation = true;
	// Amount of force added to move the player left and right.
	public float moveForce = 60f;
	// The fastest the player can travel in the x axis.
	public float maxSpeed = 5f;
	// Amount of drag that slows the player down on ground
	public float movingDrag = 10f;
	// Amount of jumping drag that slows the player down in air
	public float jumpingDrag = 2f;
	// Amount of force added when the player jumps.
	public float jumpForce = 500f;
	// Amount of force added when the player dashes.
	public float dashForce = 30f;
	// Amount of force added when the player uses Charge or Long Dash, a power up.
	public float chargeForce = 80f;
	public float respawnTime = 4.0f;
	// If the camera transitions form the player to the enemy, the player freezes.
	public bool freeze = false;
	// Accessed by ReloadLevel
	public bool died = false;
	// Accessed by the TouchInputManager to determine if the player is grounded.
	public bool grounded = false;
	// Accessed by the TouchInputManager to determine whether the player can jump
	public bool jump = false;
	// Accessed by the TouchInputManager to determine whether dash or not
	public bool isMoving;
	// Flip the sprite based on direction.
	// Accessed by TouchInputManager on mobile devices.
	public float direction = 1.0f;
	// The cool down time it takes for the player to reuse dash
	public float dashCoolDownTime = 1.0f;
	// Used for calculating the next dash time
	private float lastDashTime = 0;
	// Used for determing the direction player is facing
	private bool facingRight = true;
	// If dashRight, then add foce to the right to dash the player.
	private bool dashLeft = false;
	// If dashRight, then add foce to the right to dash the player.
	private bool dashRight = false;
	// Used to control power ups
	private bool canRoll = true;
	private bool _canCharge;


	/*============= AudioClips =============*/

	// This audio source is exclusively used for background music
	public AudioSource backgroundMusic;
	// This audio is used to play audio clips such as jumpAudio, deathAudio and endSceneAudio
	public AudioSource audioEffects;
	public AudioClip jumpAudio;
	public AudioClip deathAudio;
	public AudioClip endSceneAudio;

	/*============= Decorations =============*/

	public GameObject jumpDust;
	private bool instantiatedDustOnce = false;

	// Positional marks used to check if the player is grounded.
	private Transform groundCheck1;
	private Transform groundCheck2;
	// A positional mark where to check if the player is touching the left wall.
	private Transform leftWallJumpCheck;
	// A positional mark where to check if the player is touching the right wall.
	private Transform rightWallJumpCheck;
	// Reference to the player's PlayerGraphics child object, which holds sprite renderer and animator
	private Transform playerGraphics;
	private Animator anim;
	private Rigidbody2D rigid;
	// Touch control
	private CNAbstractController MovementJoystick;


	void Awake ()
	{
		player = this;

		groundCheck1 = transform.Find ("GroundCheck1");
		groundCheck2 = transform.Find ("GroundCheck2");
		leftWallJumpCheck = transform.Find ("WallJumpLeft");
		rightWallJumpCheck = transform.Find ("WallJumpRight");

		playerGraphics = transform.FindChild ("PlayerGraphics");
		anim = playerGraphics.GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody2D> ();

		if (!GameObject.FindGameObjectWithTag ("SpawnLocation"))
			transform.position = GameObject.FindGameObjectWithTag ("SpawnLocation").transform.position;
		else
			Debug.Log ("No SpawnLocation found");

	}

	void Start ()
	{
//		if (spawnAnimation) {
//			anim.SetTrigger ("Respawn");
//		}

		// respawnTime player is transparent
		//		playerGraphics.GetComponent<SpriteRenderer> ().color = new Color (graphicColor.r, graphicColor.g, graphicColor.b, 0.4f);

	}

	void Update ()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		bool grounded1 = Physics2D.Linecast (
			                 transform.position, 
			                 groundCheck1.position, 
			                 1 << LayerMask.NameToLayer ("Ground") |
			                 1 << LayerMask.NameToLayer ("Object"));  
		bool grounded2 = Physics2D.Linecast (
			                 transform.position, 
			                 groundCheck2.position, 
			                 1 << LayerMask.NameToLayer ("Ground") |
			                 1 << LayerMask.NameToLayer ("Object"));  
		bool leftWallTouched = Physics2D.Linecast (
			                       transform.position, 
			                       leftWallJumpCheck.position, 
			                       1 << LayerMask.NameToLayer ("Ground") |
			                       1 << LayerMask.NameToLayer ("Object"));  
		bool rightWallTouched = Physics2D.Linecast (
			                        transform.position, 
			                        rightWallJumpCheck.position, 
			                        1 << LayerMask.NameToLayer ("Ground") |
			                        1 << LayerMask.NameToLayer ("Object")); 
		grounded = grounded1 || grounded2;


		if ((Input.GetButtonDown ("Jump") || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W) || TouchInputManager.touchInputManager.SwipeUp) 
			&& (grounded || leftWallTouched || rightWallTouched))
			jump = true;


		// double Tap to dash
		if ((Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) &&
		    (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)))
			DashRight ();


		if ((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) &&
		    (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)))
			DashLeft ();

		// Put animations in Update() for frame consistancy
		if (grounded) {
			anim.SetBool ("Jump", false);
			if (!instantiatedDustOnce) { // Create jump dust
				Instantiate (jumpDust, new Vector3 (transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
				instantiatedDustOnce = true;
			}
		} else {
			anim.SetBool ("Jump", true);
			instantiatedDustOnce = false;
		}

		// Cloaking ability change player color
		//		if (_canCloak) {
		//			playerGraphics.GetComponent<SpriteRenderer> ().color = new Color (graphicColor.r, graphicColor.g, graphicColor.b, 0.4f);
		//		} else {
		//			playerGraphics.GetComponent<SpriteRenderer> ().color = graphicColor;
		//
		//		}
		//			
		//					
	}

	void FixedUpdate ()
	{
		if (!died) {
			if (!freeze) {
				float mobile_control_h = CNJoystick.joystick.GetAxis ("Horizontal");
				float h = (Mathf.Abs (mobile_control_h) > 0.05) ? mobile_control_h : Input.GetAxis ("Horizontal");

				anim.SetFloat ("Speed", Mathf.Abs (h));


				// If the player is changing direction or hasn't reached maxSpeed, allow add force 
				if (h * rigid.velocity.x < maxSpeed)
					rigid.AddForce (Vector2.right * h * moveForce);

				// If the player's horizontal velocity is greater than the maxSpeed, set the velocity to the maxSpeed
				if (Mathf.Abs (rigid.velocity.x) > maxSpeed)
					rigid.velocity = new Vector2 (Mathf.Sign (rigid.velocity.x) * maxSpeed, rigid.velocity.y);

				if (h > 0 && !facingRight)
					Flip ();
				else if (h < 0 && facingRight)
					Flip ();


				if (jump) {
					// Play a jump audio clip.
					audioEffects.clip = jumpAudio;
					audioEffects.Play ();
					rigid.AddForce (new Vector2 (0f, jumpForce));
					//rigidbody2D.AddForce(new Vector2(jumpForce * direction /2, jumpForce));
					jump = false;
				}
					

				//decrease the linearDrag if the player is jumping
				if (grounded)
					rigid.drag = movingDrag;
				else
					rigid.drag = jumpingDrag;


//				if (dashLeft) {
//					Debug.Log ("Dash left");
//					if (canRoll)
//						rigid.AddForce (new Vector2 (-dashForce, 1), ForceMode2D.Impulse);
//					else if (_canCharge)
//						rigid.AddForce (new Vector2 (-chargeForce, 2), ForceMode2D.Impulse);
//
//					dashLeft = false;
//				}
//
//				if (dashRight) {
//					Debug.Log ("Dash right");
//					if (canRoll)
//						rigid.AddForce (new Vector2 (dashForce, 1), ForceMode2D.Impulse);
//					else if (_canCharge)
//						rigid.AddForce (new Vector2 (chargeForce, 2), ForceMode2D.Impulse);
//
//					dashRight = false;
//				}


			}
		} else {
			// If the player dies and falls on the ground freeze the player
			if (grounded) {
				rigid.isKinematic = true;
				Destroy (GetComponent<Collider2D> ());
			}
		}
	}

	public void Death ()
	{
		if (!died) {
			anim.SetTrigger ("Death");
			// Play death music
			backgroundMusic.Stop ();
			audioEffects.clip = deathAudio;
			audioEffects.Play ();
			// Death bounce like in Mario
			rigid.AddForce (new Vector2 (0, 400));
			Destroy (GetComponent<Collider2D> ());
			died = true;
		} 
	}

	public void Death (int bounceUpForce)
	{
		if (!died) {
			anim.SetTrigger ("Death");
			// Play death music
			backgroundMusic.Stop ();
			audioEffects.clip = deathAudio;
			audioEffects.Play ();
			// Death bounce like in Mario
			rigid.AddForce (new Vector2 (0, bounceUpForce));
			Destroy (GetComponent<Collider2D> ());
			died = true;
		} 
	}


	/************************ Mobile Control  ************************/

	// Called by TouchInputManager to dash left and right
	public void DashLeft ()
	{
		if ((Time.time - lastDashTime) > dashCoolDownTime && direction < 0) {
			dashLeft = true;
			lastDashTime = Time.time;
		}
	}

	public void DashRight ()
	{
		if ((Time.time - lastDashTime) > dashCoolDownTime && direction > 0) {
			dashRight = true;
			lastDashTime = Time.time;
		}
	}

	/*****************************************************************/


	void OnTriggerEnter2D (Collider2D other)
	{


	}

	void OnCollisionEnter2D (Collision2D coll)
	{

	}


	// Called by scene loader
	public void EndScene (bool playEndSceneAudio)
	{
		// Play end scene music
		backgroundMusic.Stop ();
		audioEffects.clip = endSceneAudio;
		if (playEndSceneAudio)
			audioEffects.Play ();

		playerGraphics.GetComponent<SpriteRenderer> ().enabled = false;

		// Then freeze the player
		rigid.isKinematic = true;
		Destroy (GetComponent<Collider2D> ());
	}

	void Flip ()
	{
		direction *= -1;
		facingRight = !facingRight;
		Vector3 theScale = playerGraphics.localScale;
		theScale.x *= -1;
		playerGraphics.localScale = theScale;
	}

	IEnumerator DestroySelf ()
	{
		yield return new WaitForSeconds (respawnTime);
		Transform.Destroy (transform.gameObject);
	}

	void DashPowerUp ()
	{
		canRoll = true;

		Debug.Log ("Switched to dash power up");
		// Dash power up has shorter charging time, but less dashing power
		dashCoolDownTime = 0.7f;
	}

	// Charge or Long dash
	void ChargePowerUp ()
	{
		_canCharge = true;

		Debug.Log ("Switched to charge power up");
		// Charge power up has a longer cool down time
		dashCoolDownTime = 3f;
	}

}