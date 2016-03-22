using UnityEngine;
using System.Collections;

public class PhoneButton : MonoBehaviour {
	private PhoneCloseUp phoneScript;

	// Use this for initialization
	void Start () {
		phoneScript = GameObject.Find("PhoneCloseUp").GetComponent<PhoneCloseUp>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		phoneScript.AppendPhoneDigit(this.name);
	}
}
