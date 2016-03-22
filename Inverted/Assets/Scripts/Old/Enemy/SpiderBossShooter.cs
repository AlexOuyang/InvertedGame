using UnityEngine;
using System.Collections;

public class SpiderBossShooter : MonoBehaviour {
	// public
	public float projectileVelocity = 15.0f; // in metres per second
	public GameObject projPrefab;
	public float fireRate = 0.1f;
	public float scatteredness = 1.0f;
	private bool isActive = false;
	public AudioSource shootingAudio;         
	
	// private
	private float fireTimer;

	public bool IsActive{
		get { return isActive; }
		set { isActive = value; }
	}
	// Use this for initialization
	void Start () {
		fireTimer = Time.time + fireRate;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > fireTimer && isActive){
			GameObject projectile;
			Vector3 velocityVector = -transform.up;
			
			if (scatteredness != 0){
				Vector2 rand = Random.insideUnitCircle;
				velocityVector += new Vector3(rand.x, rand.y, 0) * scatteredness;
			}
			
			velocityVector = velocityVector.normalized * projectileVelocity;
			
			projectile = Instantiate(projPrefab, transform.position, Quaternion.identity) as GameObject;
			shootingAudio.Play();
			projectile.GetComponent<ProjectileScript>().velocityVector = velocityVector;
			fireTimer = Time.time + fireRate;
		}
	}
}