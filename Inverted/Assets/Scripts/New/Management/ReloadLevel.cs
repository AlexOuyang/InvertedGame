using UnityEngine;
using System.Collections;

// Is attached to the GameManager, it opens up a dialogue upon player's death and asking if the player wnats to
// save and exit, or respawn.
public class ReloadLevel : MonoBehaviour
{
	private float waitForReload = 4;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerControl.player.died) {
			StartCoroutine ("ReloadGame");
		}

		// Testing
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	IEnumerator ReloadGame ()
	{
		yield return new WaitForSeconds (waitForReload);
		Application.LoadLevel (Application.loadedLevel);
	}

	// Saves the player progress to disk
	void OnApplicationQuit ()
	{
		//Debug.Log("Save");
//		Debug.Log("On Exit: Total number of Powerups: " + PlayerPrefs.GetInt("numOfPowerUpsActivated"));
//		Debug.Log("On Exit: The currently selected PowerUp: " + PlayerPrefs.GetInt("currentlySelectedPowerUp"));

		PlayerPrefs.Save ();
	}

	public void ReloadDuringRunTime ()
	{
		Application.LoadLevel (Application.loadedLevel);

	}
}
