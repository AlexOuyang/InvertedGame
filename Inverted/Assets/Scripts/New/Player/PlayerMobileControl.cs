using UnityEngine;
using System.Collections;

public class PlayerMobileControl : MonoBehaviour {
	
	public float moveForce = 20.0f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 4.0f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 270.0f;			// Amount of force added when the player jumps.
	//public float tauntProbability = 50f;	// Chance of a taunt happening.
	//public float tauntDelay = 1f;			// Delay for when the taunt should happen.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	//public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public bool died = false;             //died 
	public AudioSource backgroundMusic;
	public AudioSource deathMusic;
	
	//private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.
	private bool facingRight = true;			// For determining which way the player is currently facing.
	private bool jump = false;				// Condition for whether the player should jump.
	private float direction = 1.0f;
	private float lastTouchPlayerTime;
	
	
	void Awake(){
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}
	
	void Start(){
		backgroundMusic.Play();
		
	}
	
	void Update(){
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground") |1 << LayerMask.NameToLayer("Object"));  
		
		if(Input.GetButtonDown("Jump") && grounded)
			jump = true;
		
	}
	
	
	void FixedUpdate (){
		if(!died){
			float h = Input.GetAxis("Horizontal");
			//running animation
			anim.SetFloat("Speed", Mathf.Abs(h));
			
			// If the player is changing direction or hasn't reached maxSpeed, add a force to the player
			if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
			
			// If the player's horizontal velocity is greater than the maxSpeed, set the velocity to the maxSpeed
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
				GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
			
			if(h > 0 && !facingRight){
				Flip();
			}else if(h < 0 && facingRight){
				Flip();
			}
			
			if(jump){
				//			// Set the Jump animator trigger parameter.
				anim.SetTrigger("Jump");
				//
				//			// Play a random jump audio clip.
				//			int i = Random.Range(0, jumpClips.Length);
				//			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
				
				//rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				GetComponent<Rigidbody2D>().AddForce(new Vector2(jumpForce * direction /2, jumpForce));
				jump = false;
			}
		}else{
			//anim.SetTrigger("death");
			//transform.collider2D.enabled = false;
			//Destroy(rigidbody2D);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Obstacles"){
			Death();
		}
		if (coll.gameObject.tag == "Porcupine"){
			Death();
			if (Time.time > lastTouchPlayerTime + 1) {
				coll.gameObject.SendMessage("Flip");
				lastTouchPlayerTime = Time.time;
			} 
			
		}
	}
	
	void Death(){
		died = true;
		anim.SetTrigger("Death");
		deathMusic.Play();
		backgroundMusic.Stop();
	}
	
	void Flip (){
		direction *= -1;
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	
	//	public IEnumerator Taunt()
	//	{
	//		// Check the random chance of taunting.
	//		float tauntChance = Random.Range(0f, 100f);
	//		if(tauntChance > tauntProbability)
	//		{
	//			// Wait for tauntDelay number of seconds.
	//			yield return new WaitForSeconds(tauntDelay);
	//
	//			// If there is no clip currently playing.
	//			if(!audio.isPlaying)
	//			{
	//				// Choose a random, but different taunt.
	//				tauntIndex = TauntRandom();
	//
	//				// Play the new taunt.
	//				audio.clip = taunts[tauntIndex];
	//				audio.Play();
	//			}
	//		}
	//	}
	//
	//
	//	int TauntRandom()
	//	{
	//		// Choose a random index of the taunts array.
	//		int i = Random.Range(0, taunts.Length);
	//
	//		// If it's the same as the previous taunt...
	//		if(i == tauntIndex)
	//			// ... try another random taunt.
	//			return TauntRandom();
	//		else
	//			// Otherwise return this index.
	//			return i;
	//	}
}
