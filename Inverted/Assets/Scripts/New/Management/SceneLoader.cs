using UnityEngine;
using System.Collections;

/// <summary>
/// Scene loader is used to load the next scene after one scene ends
/// </summary>
public class SceneLoader : MonoBehaviour
{
	public string sceneName;
	public bool enterCutScene = false;
	public float timeUntilCutScene = 10f;
	public bool cutScene = false;
	public float cutSceneDuration = 10f;
	public bool endSceneMusic = true;
	private float loadingTime = 5f;
	private float loadingTimeNoMusic = 1f;

	// Use this for initialization
	void Start ()
	{
		if (cutScene && enterCutScene) {
			Debug.LogError ("SceneLoader can not have both cutScene and enterCutScene be true");
		}
		if (cutScene) {
			StartCoroutine (CutSceneTime (cutSceneDuration));
		}

		if (enterCutScene) {
			StartCoroutine (WaitTillCutScene (timeUntilCutScene));
		}

	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			if (ScreenFader.fader != null)
				ScreenFader.fader.FadeToBlack ();

			PlayerControl.Player.EndScene (endSceneMusic);
			if (endSceneMusic) {
				StartCoroutine (LoadAnotherScene (loadingTime));
			} else {
				StartCoroutine (LoadAnotherScene (loadingTimeNoMusic));
			}
		}
	}

	// Waiting to play the end scene music then load another level
	IEnumerator LoadAnotherScene (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	// Waiting to finish the cut scene then load another scene
	IEnumerator CutSceneTime (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);

		if (ScreenFader.fader != null)
			ScreenFader.fader.FadeToBlack ();
		PlayerControl.Player.EndScene (endSceneMusic);
		if (endSceneMusic) {
			StartCoroutine (LoadAnotherScene (loadingTime));
		} else {
			StartCoroutine (LoadAnotherScene (loadingTimeNoMusic));
		}
	}

	// Waiting until the cut scene begin then load cut scene
	IEnumerator WaitTillCutScene (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		
		if (ScreenFader.fader != null)
			ScreenFader.fader.FadeToBlack ();

		StartCoroutine (LoadAnotherScene (loadingTimeNoMusic));
		
	}

	// Called by other objects used to load the next scene
	public void LoadAnotherScene ()
	{
		if (ScreenFader.fader != null)
			ScreenFader.fader.FadeToBlack ();
				
		PlayerControl.Player.EndScene (endSceneMusic);
		if (endSceneMusic) {
			StartCoroutine (LoadAnotherScene (loadingTime));
		} else {
			StartCoroutine (LoadAnotherScene (loadingTimeNoMusic));
		}
	}
}
