using UnityEngine;
using System.Collections;

public class PhoneCloseUp : MonoBehaviour {
	public GameObject Phone;

	private GameObject sceneLoader;
	private Transform player;
	static private string phoneNumber = "6197499248";
	private string guessedPhoneNumber;

	void Awake() {
		sceneLoader = GameObject.Find("LevelLoader");
		sceneLoader.GetComponent<Collider2D>().enabled = false;
	}

	// Use this for initialization
	void Start () {
		guessedPhoneNumber = "";

		player = GameObject.FindGameObjectWithTag ("Player").transform;

		Debug.Log(sceneLoader);
	
	}
	
	// Update is called once per frame
	void Update(){
		Debug.Log(guessedPhoneNumber);
		if(guessedPhoneNumber.Equals(phoneNumber)) {
			sceneLoader.GetComponent<Collider2D>().enabled = true;
			bringDownThePhone();
		}
		
	}

	void OnMouseDown() {
		bringDownThePhone();
	}

	void bringDownThePhone() {
		
		player.GetComponent<PlayerControl>().freeze = false; // Also freeze the player
		
		Phone.SetActive(true);
		
		guessedPhoneNumber = "";  // Reset the phone number
		
		this.gameObject.SetActive(false);
	}
	
	public void AppendPhoneDigit(string digit){
		guessedPhoneNumber += digit;
	}
}
