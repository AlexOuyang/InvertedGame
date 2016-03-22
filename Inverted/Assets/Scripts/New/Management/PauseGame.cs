﻿using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	private bool paused = false;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.P))
		{
			Pause();
		}

//		if(paused)
//			Time.timeScale = 0;
//		else
//			Time.timeScale = 1;
	}

	void Pause() {
		paused = !paused;

		if(paused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}
}
