using UnityEngine;
using System.Collections;

public class PatrolEnemyControl : MonoBehaviour
{
	public float ChaseRange = 3.0f;
	public float AttackRange = 0.2f;
	public float ChasingSpeed = 15.0f;
	public float PatrollingSpeed = 1.0f;
	private bool died = false;
	private float startPositionX;
	private Vector3 startPosition;
	private float PlayerDistanceFromStartingPosition;
	private float PlayerDistanceFromEnemy;
	private Transform Target;
	private float Xdifference;
	private float Ydifference;
	private float direction = 1.0f;
	private PlayerControl player;
	//get the PlayerControl script
	private bool isChasing = true;
	private Vector2 TargetDirection = Vector2.zero;

	
	//Audios
	//var shootingAudio: AudioSource;
	void Start ()
	{
		startPosition = transform.position;
		startPositionX = transform.position.x;
	}

	void Awake ()
	{
		Target = GameObject.FindGameObjectWithTag ("Player").transform;
		player = Target.GetComponent<PlayerControl> ();
	}

	void Update ()
	{
		if (!died) {
			if (!player.died) {
				PlayerDistanceFromStartingPosition = Vector2.Distance (Target.transform.position, startPosition);
				PlayerDistanceFromEnemy = Vector2.Distance (Target.transform.position, transform.position);
				if (PlayerDistanceFromStartingPosition > ChaseRange) {
					Patrolling ();
					isChasing = false;
				}
				if (PlayerDistanceFromStartingPosition < ChaseRange) {
					Chase ();
					isChasing = true;
				}
			} else {
				//do something else
				isChasing = false;

			}
		} else {
			Death ();
		}

	}

	void Chase ()
	{
		Debug.Log ("Chasing");
		Xdifference = Target.position.x - transform.position.x;
		Ydifference = Target.position.y - transform.position.y;
		TargetDirection = new Vector2 (Xdifference, Ydifference);
		GetComponent<Rigidbody2D> ().AddForce (TargetDirection.normalized * ChasingSpeed);
		if (TargetDirection.x > 0) {
			//Debug.Log("Looking Right");
		}
		if (TargetDirection.x < 0) {
			//Debug.Log("Looking Left");
		}
	}

	void Attack ()
	{
//		if (Time.time > NextAttack){
//			shootingAudio.Play();
//			Target.SendMessage("ApplyDamage", Damage);
//			Debug.Log("The Enemy Has Attacked");//testing purpose
//			NextAttack = Time.time + AttackRate;
//			
//			animation.Play("ReaperAttack",PlayMode.StopAll);
//		}
	}

	void Patrolling ()
	{
		Debug.Log ("out of range");
		if (transform.position.x > startPositionX + ChaseRange || transform.position.x < startPositionX - ChaseRange) { 
			Flip ();
		} 
		transform.position = new Vector2 (transform.position.x + (PatrollingSpeed * direction) * Time.deltaTime, transform.position.y); 
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Obstacles") {
			died = true;
			//Debug.Log (died);
		}
	}

	void Death ()
	{

	}

	void Flip ()
	{
		direction *= -1;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		//touchPlayer = false;
	}

}
