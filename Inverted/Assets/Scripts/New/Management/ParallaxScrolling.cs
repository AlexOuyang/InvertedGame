using UnityEngine;
using System.Collections;

//this script is attached to the Background gameobjects, it accesses all the childobjects of Background
//and assigns a parallaxing scales to each one of them based on their posiion.z value;
public class ParallaxScrolling : MonoBehaviour {
	//a collection of the transformation of backgrounds
	public Transform[] backgroundsTrans;
	//the parallaxing speed of each backgrounds
	private float[] parallaxingScales;
	public float parallaxingSpeed = 2.0f;
	public float smoothing = 1.0f;
	private Transform mainCamTrans;
	private Vector3 previousCamPos;

	void Awake () {
		mainCamTrans = Camera.main.transform;
		//get all the backgrounds in Bakground game object
		backgroundsTrans = GetComponentsInChildren<Transform>();
	}

	void Start () {
		previousCamPos = mainCamTrans.position;

		parallaxingScales = new float[backgroundsTrans.Length];
		//assigning corresponding parallaxingscales to each backgroud
		for (int i = 0; i<backgroundsTrans.Length; i++) {
			//the parallaxing scales are depended on its z position of the backgrounds
			parallaxingScales[i] = backgroundsTrans[i].position.z * (-parallaxingSpeed);
		}
	}
	
	void Update () {
		for (int i = 0; i<parallaxingScales.Length; i++) {
			float parallaxingOffset = (previousCamPos.x - mainCamTrans.position.x) * parallaxingScales[i];
			//reset each backgrounds position based on parallaxing scales.
			Vector3 targetParallaxingPos = new Vector3(backgroundsTrans[i].position.x + parallaxingOffset, backgroundsTrans[i].position.y, backgroundsTrans[i].position.z);
			//lerp the background position to the targetParrallaxingPosition
			backgroundsTrans[i].position = Vector3.Lerp(backgroundsTrans[i].position, targetParallaxingPos, Time.deltaTime * smoothing);
		}

		//set the previous Camera position to the camera's position at the end of the frame;
		previousCamPos = mainCamTrans.position;
	}
}
