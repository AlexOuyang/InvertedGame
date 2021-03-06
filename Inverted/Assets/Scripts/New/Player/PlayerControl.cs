﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Player control governs the player behavior.
/// </summary>
public class PlayerControl : MonoBehaviour
{
	// Static reference to the playerControl script
	public static PlayerControl Player;


	/*========= Player Attributes =========*/

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
	// These are the layer mask index of platform(ground), enemy and objects,
	// only allow raycast to hit platform, objects and enemy
	public int groundLayerIdx = 8;
	public int enemyLayerIdx = 9;
	public int objectLayerIdx = 10;

	// Used for calculating the next dash time
	private float _lastDashTime = 0;
	// Used for determing the direction player is facing
	private bool _facingRight = true;
	// Used for player rolling.
	private bool _rollLeft = false;
	private bool _rollRight = false;
	// Used to control power ups
	private bool _canRoll = true;
	private bool _canCharge;
	private bool _instantiateDustOnce = false;
	// The amount of time player can not move when shooting the arrow
	private float _arrowShootingFreezeTime = 0.6f;
	// The amount of time it takes for the arrow remains and arrow trajectory to be produced after shooting starts
	private float _arrowShootingBufferingTime = 0.3f;
	// Used to restrict arrow shooting frequencies
	private bool _canShootArrow = true;
	// Used to allow shooting in FixedUpdate
	private bool _shootArrow = false;


	/*============= AudioClips =============*/

	// This audio source is exclusively used for background music
	public AudioSource backgroundMusic;
	// This audio is used to play audio clips such as jumpAudio, deathAudio and endSceneAudio
	public AudioSource audioEffects;
	public AudioClip jumpAudio;
	public AudioClip deathAudio;
	public AudioClip endSceneAudio;


	/*============= Public Objects =============*/

	public GameObject jumpDust;
	public GameObject arrow;
	public GameObject bowLine;


	// Used to launch the arrow
	private Transform _arrowLauncher;
	// Positional marks used to check if the player is grounded.
	private Transform _groundCheck1;
	private Transform _groundCheck2;
	// A positional mark where to check if the player is touching the left wall.
	private Transform _leftWallJumpCheck;
	// A positional mark where to check if the player is touching the right wall.
	private Transform _rightWallJumpCheck;
	// Reference to the player's PlayerGraphics child object, which holds sprite renderer and animator
	private Transform _playerGraphics;
	private Animator _anim;
	private Rigidbody2D _rigidbody;


	public float UI_Left = 0;
	public float UI_Right = 0;

	void Awake ()
	{
		Player = this;

		_groundCheck1 = transform.Find ("GroundCheck1");
		_groundCheck2 = transform.Find ("GroundCheck2");
		_leftWallJumpCheck = transform.Find ("WallJumpLeft");
		_rightWallJumpCheck = transform.Find ("WallJumpRight");

		_playerGraphics = transform.FindChild ("PlayerGraphics");
		_anim = _playerGraphics.GetComponent<Animator> ();
		_rigidbody = GetComponent<Rigidbody2D> ();

		if (!GameObject.FindGameObjectWithTag ("SpawnLocation"))
			transform.position = GameObject.FindGameObjectWithTag ("SpawnLocation").transform.position;
		else
			Debug.Log ("No SpawnLocation found");

		_arrowLauncher = transform.FindChild ("ArrowLauncher");

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
			                 _groundCheck1.position, 
			                 1 << LayerMask.NameToLayer ("Ground") |
			                 1 << LayerMask.NameToLayer ("Object"));  
		bool grounded2 = Physics2D.Linecast (
			                 transform.position, 
			                 _groundCheck2.position, 
			                 1 << LayerMask.NameToLayer ("Ground") |
			                 1 << LayerMask.NameToLayer ("Object"));  
		bool leftWallTouched = Physics2D.Linecast (
			                       transform.position, 
			                       _leftWallJumpCheck.position, 
			                       1 << LayerMask.NameToLayer ("Ground") |
			                       1 << LayerMask.NameToLayer ("Object"));  
		bool rightWallTouched = Physics2D.Linecast (
			                        transform.position, 
			                        _rightWallJumpCheck.position, 
			                        1 << LayerMask.NameToLayer ("Ground") |
			                        1 << LayerMask.NameToLayer ("Object")); 
		grounded = grounded1 || grounded2;

		// TouchInputManager.touchInputManager.Tap
		if ((Input.GetButtonDown ("Jump") || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))
		    && (grounded))
			jump = true;


		// double Tap to dash
		if (((Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) &&
		    (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift))) ||
		    TouchInputManager.touchInputManager.SwipeRight)
			rollRight ();

		if (((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) &&
		    (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift))) ||
		    TouchInputManager.touchInputManager.SwipeLeft)
			rollLeft ();

		if (grounded) {
			_anim.SetBool ("IsJumping", false);
			if (!_instantiateDustOnce) { // Create jump dust
				Instantiate (jumpDust, new Vector3 (transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
				_instantiateDustOnce = true;
			}
		} else {
			_anim.SetBool ("IsJumping", true);
			_instantiateDustOnce = false;
		}

		if (Input.GetKeyDown (KeyCode.X) && _canShootArrow) {
			_anim.SetTrigger ("Shooting");
			_shootArrow = true;
//			GameObject arrowShot = Instantiate (arrow, new Vector3 (_arrowLauncher.position.x, _arrowLauncher.position.y, _arrowLauncher.position.z), Quaternion.identity) as GameObject;
//			arrowShot.GetComponent<ArrowShooting>().Flip(this.direction);
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

//				// UI controls
//				if (UI_Left == 1 && UI_Right == 0)
//					mobile_control_h = -1;
//				else if (UI_Left == 0 && UI_Right == 1)
//					mobile_control_h = 1;
//				else
//					mobile_control_h = 0;
				
				float h = (Mathf.Abs (mobile_control_h) > 0.05) ? mobile_control_h : Input.GetAxis ("Horizontal");

//				if (h > 0.05f)
//					h = 0.9f;
//				else if (h < -0.05f)
//					h = -0.9f;
//				
				_anim.SetFloat ("Speed", Mathf.Abs (h));



				// If the player is changing direction or hasn't reached maxSpeed, allow add force 
				if (h * _rigidbody.velocity.x < maxSpeed)
					_rigidbody.AddForce (Vector2.right * h * moveForce);

				// If the player's horizontal velocity is greater than the maxSpeed, set the velocity to the maxSpeed
				if (Mathf.Abs (_rigidbody.velocity.x) > maxSpeed)
					_rigidbody.velocity = new Vector2 (Mathf.Sign (_rigidbody.velocity.x) * maxSpeed, _rigidbody.velocity.y);

				if (h > 0 && !_facingRight)
					Flip ();
				else if (h < 0 && _facingRight)
					Flip ();


				if (jump) {
					// Play a jump audio clip.
					audioEffects.clip = jumpAudio;
					audioEffects.Play ();
					_rigidbody.AddForce (new Vector2 (0f, jumpForce));
					//rigidbody2D.AddForce(new Vector2(jumpForce * direction /2, jumpForce));
					jump = false;
				}
					
				// The playerVelocityY determines whether the player is jumping up or falling down.
				float playerVelocityY = _rigidbody.velocity.y;
				//decrease the linearDrag if the player is jumping
				if (grounded) {
//					anim.SetBool ("Jump", false);
					_anim.SetFloat ("PlayerJumpUpSpeed", 0f);
					_rigidbody.drag = movingDrag;
				} else {
//						anim.SetBool ("Jump", true);
					_anim.SetFloat ("PlayerJumpUpSpeed", playerVelocityY);
					_rigidbody.drag = jumpingDrag;
				}

				if (_rollLeft) {
					if (_canRoll)
						_rigidbody.AddForce (new Vector2 (-dashForce, 1), ForceMode2D.Impulse);
					else if (_canCharge)
						_rigidbody.AddForce (new Vector2 (-chargeForce, 2), ForceMode2D.Impulse);

					_rollLeft = false;
				}

				if (_rollRight) {
					if (_canRoll)
						_rigidbody.AddForce (new Vector2 (dashForce, 1), ForceMode2D.Impulse);
					else if (_canCharge)
						_rigidbody.AddForce (new Vector2 (chargeForce, 2), ForceMode2D.Impulse);

					_rollRight = false;
				}

				// Bowing mechanism
				if (_shootArrow) {
					_shootArrow = false;
					StartCoroutine (bowing ());
				}


			}
		} else {
			// If the player dies and falls on the ground freeze the player
			if (grounded) {
				_rigidbody.isKinematic = true;
				Destroy (GetComponent<Collider2D> ());
			}
		}
	}


	void OnTriggerEnter2D (Collider2D other)
	{
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
	}


	void rollLeft ()
	{
		if ((Time.time - _lastDashTime) > dashCoolDownTime && direction < 0) {
			_rollLeft = true;
			_lastDashTime = Time.time;
		}
	}

	void rollRight ()
	{
		if ((Time.time - _lastDashTime) > dashCoolDownTime && direction > 0) {
			_rollRight = true;
			_lastDashTime = Time.time;
		}
	}


	void Flip ()
	{
		Debug.Log (_playerGraphics.localScale.x);
		direction *= -1;
		_facingRight = !_facingRight;
//		Vector3 theScale = playerGraphics.localScale;
//		theScale.x *= -1;
//		playerGraphics.localScale.x *= -1;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	// This governs the bowing freeze time
	IEnumerator bowingFreeze ()
	{
		_anim.SetBool ("IsShooting", true); // Used to prevent jumping animation from overriding Shooting animation
		freeze = true;
		_canShootArrow = false;
		jump = false; // used to prevent user from juppming after shooting arrow because jump tapped before shooting arrow
		yield return new WaitForSeconds (_arrowShootingFreezeTime);
		jump = false; // used to prevent user from juppming after shooting arrow because jump tapped before shooting arrow
		freeze = false;
		_canShootArrow = true;
		_anim.SetBool ("IsShooting", false);
	}


	// Main bowing mechanism, instantiates arrow remainds and produces the bowline.
	IEnumerator bowing ()
	{
		StartCoroutine (bowingFreeze ());

		float offsetTime = 0.05f;
		yield return new WaitForSeconds (_arrowShootingBufferingTime - offsetTime);

		// Bit shift the index of the layer to include only below collision layers
		int collisionLayerMask = 1 << groundLayerIdx | 1 << enemyLayerIdx | 1 << objectLayerIdx;
		Vector3 rayDirection = new Vector3 (this.direction, 0, 0);
		Vector3 offset = new Vector3 (this.direction / 2, 0, 0);
		RaycastHit2D hit = Physics2D.Raycast (_arrowLauncher.transform.position, (Vector2)rayDirection, Mathf.Infinity, collisionLayerMask);
		if (hit.collider != null) {
			float distance = Mathf.Abs (hit.point.x - _arrowLauncher.transform.position.x);
			Debug.DrawRay (_arrowLauncher.transform.position, distance * rayDirection, Color.green, 3f, false);

			// Create trajectory
			GameObject trajectory = Instantiate (bowLine, hit.point, Quaternion.identity) as GameObject;
			float scale_x = trajectory.GetComponent<Renderer> ().bounds.size.x;
			Vector3 newScale = trajectory.transform.localScale;
			newScale.x = (-1) * this.direction * distance * newScale.x / scale_x;
			trajectory.transform.localScale = newScale;

			// Create arrow remains
			yield return new WaitForSeconds (offsetTime);

			Quaternion groundNormal = Quaternion.FromToRotation (this.direction * Vector3.left, hit.normal);
			float rotationOffsetMultiplier = Random.Range (3f, 6f); // create rotation to ground normal variations
			groundNormal.z /= rotationOffsetMultiplier;
			GameObject arrowShot = Instantiate (arrow, hit.point, groundNormal) as GameObject;
			arrowShot.GetComponent<ArrowRemains> ().Flip (this.direction);

			Destroy (trajectory);
		}
	}

	IEnumerator destroySelf ()
	{
		yield return new WaitForSeconds (respawnTime);
		Transform.Destroy (transform.gameObject);
	}
		


	/*=================== PUBLIC =====================*/

	public void Death ()
	{
		if (!died) {
			_anim.SetTrigger ("Death");
			// Play death music
			backgroundMusic.Stop ();
			audioEffects.clip = deathAudio;
			audioEffects.Play ();
			// Death bounce like in Mario
			_rigidbody.AddForce (new Vector2 (0, 400));
			Destroy (GetComponent<Collider2D> ());
			died = true;
		} 
	}

	public void Death (int bounceUpForce)
	{
		if (!died) {
			_anim.SetTrigger ("Death");
			// Play death music
			backgroundMusic.Stop ();
			audioEffects.clip = deathAudio;
			audioEffects.Play ();
			// Death bounce like in Mario
			_rigidbody.AddForce (new Vector2 (0, bounceUpForce));
			Destroy (GetComponent<Collider2D> ());
			died = true;
		} 
	}

	// Called by scene loader
	public void EndScene (bool playEndSceneAudio)
	{
		// Play end scene music
		backgroundMusic.Stop ();
		audioEffects.clip = endSceneAudio;
		if (playEndSceneAudio)
			audioEffects.Play ();

		_playerGraphics.GetComponent<SpriteRenderer> ().enabled = false;

		// Then freeze the player
		_rigidbody.isKinematic = true;
		Destroy (GetComponent<Collider2D> ());
	}

	// Shoots arrow on UI button click
	public void OnClickShoot ()
	{
		if (_canShootArrow) {
			_anim.SetTrigger ("Shooting");
			_shootArrow = true;
		}
	}

	// Shoots arrow on UI button click
	public void OnClickJump ()
	{
		if (grounded)
			jump = true;
	}

	public void LeftButtonEnter ()
	{
		UI_Left = 1f;
	}

	public void LeftButtonExit ()
	{
		UI_Left = 0;
	}

	public void RightButtonEnter ()
	{
		UI_Right = 1f;
	}

	public void RightButtonExit ()
	{
		UI_Right = 0;
	}
}