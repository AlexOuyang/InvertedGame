﻿using UnityEngine;
using System.Collections;

public class HiResScreenShots : MonoBehaviour {
	public int resWidth = 2550; 
	public int resHeight = 3300;

	private float runTime = 10.0f;

	public Camera camera;

	private int numOfPicsTaken = 0;

	private float time0 = 0;



	private bool takeHiResShot = false;
	public static string ScreenShotName(int width, int height) {
		return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	public void TakeHiResShot() {
		takeHiResShot = true;
	}


	void Update() {
//		takeHiResShot |= Input.GetKeyDown("k");
//		if (takeHiResShot) {
//			RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
//			camera.targetTexture = rt;
//			Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
//			camera.Render();
//			RenderTexture.active = rt;
//			screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
//			camera.targetTexture = null;
//			RenderTexture.active = null; // JC: added to avoid errors
//			Destroy(rt);
//			byte[] bytes = screenShot.EncodeToPNG();
//			string filename = ScreenShotName(resWidth, resHeight);
//			System.IO.File.WriteAllBytes(filename, bytes);
//			Debug.Log(string.Format("Took screenshot to: {0}", filename));
//			takeHiResShot = false;
//			numOfPicsTaken++;

		float t = Time.realtimeSinceStartup - time0;
		numOfPicsTaken++;
//		Application.CaptureScreenshot(""+numOfPicsTaken+".png");
//			if(Time.realtimeSinceStartup - time0 > runTime) {
//				takeHiResShot = false;
//				Debug.Log("numOfScreenShotsTaken: " + numOfPicsTaken);
//			}
//		}else{
//			time0 = Time.realtimeSinceStartup;
//			takeHiResShot = true;
//		}
	}
}
