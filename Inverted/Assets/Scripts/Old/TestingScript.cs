using UnityEngine;
using System.Collections;

public class TestingScript : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void loadNextLevel ()
		{
				Debug.Log ("Load next level");
				int i = Application.loadedLevel;
				Application.LoadLevel (i + 1);
		}

		public void loadPreviousLevel ()
		{
				int i = Application.loadedLevel;
				if (i > 0) {
						Application.LoadLevel (i - 1);
				}
		}
}
