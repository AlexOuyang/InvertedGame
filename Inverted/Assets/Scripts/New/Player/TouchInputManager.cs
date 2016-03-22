using UnityEngine;
using System.Collections;
using UnityEditor;

// Created by: Alex Chenxing Ouyang
// This script is attached to the player and is used to detect swipe and tap on the right screen. (Since right screen
// is designated for swiping and tapping and left screen is designated for movement control and CNController is used there)
[InitializeOnLoad]
public class TouchInputManager : MonoBehaviour
{
	public static TouchInputManager touchInputManager;

	// The InputDetectionRange defines a space on the screen in which user inputs will be detected
	public bool InputDetectionLeftScreen = false;
	public bool InputDetectionRightScreen = true;
	private int inputDetectionRangeX1;
	private int inputDetectionRangeX2;
	private int inputDetectionRangeY1;
	private int inputDetectionRangeY2;

	// Those are used for accessing swipe directions
	public bool SwipeUp { get { return SWIPE_UP; } }

	public bool SwipeDown { get { return SWIPE_DOWN; } }

	public bool SwipeLeft { get { return SWIPE_LEFT; } }

	public bool SwipeRight { get { return SWIPE_RIGHT; } }

	public bool Tap { get { return TAP; } }

	private bool SWIPE_UP = false;
	private bool SWIPE_DOWN = false;
	private bool SWIPE_LEFT = false;
	private bool SWIPE_RIGHT = false;
	private bool TAP = false;

	private float fingerStartTime = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	private bool isSwipe = false;
	private float minSwipeDist = 40.0f;
	private float maxSwipeTime = 0.5f;


	void Awake ()
	{
		touchInputManager = this;
	
	}

	void Start ()
	{
		
	}
	// Update is called once per frame
	void Update ()
	{
		// Set the camera input detection boundary
		if (InputDetectionLeftScreen && InputDetectionRightScreen) {
			inputDetectionRangeX1 = 0;
			inputDetectionRangeX2 = Screen.width;
			inputDetectionRangeY1 = 0;
			inputDetectionRangeY2 = Screen.height;
		} else if (!InputDetectionLeftScreen && InputDetectionRightScreen) {
			inputDetectionRangeX1 = Screen.width / 2;
			inputDetectionRangeX2 = Screen.width;
			inputDetectionRangeY1 = 0;
			inputDetectionRangeY2 = Screen.height;
		} else if (InputDetectionLeftScreen && !InputDetectionRightScreen) {
			inputDetectionRangeX1 = 0;
			inputDetectionRangeX2 = Screen.width / 2;
			inputDetectionRangeY1 = 0;
			inputDetectionRangeY2 = Screen.height;
		}


		// Reset control options
		SWIPE_UP = false;
		SWIPE_DOWN = false;
		SWIPE_LEFT = false;
		SWIPE_RIGHT = false;
		TAP = false;
			
		// Input detection
		if (Input.touchCount > 0) {
			
			foreach (Touch touch in Input.touches) {
				switch (touch.phase) {
				case TouchPhase.Began:
					/* this is a new touch */
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;
					
				case TouchPhase.Canceled:
					/* The touch is being canceled */
					isSwipe = false;
					break;
					
				case TouchPhase.Ended:

					if (touch.position.x > inputDetectionRangeX1 && touch.position.x < inputDetectionRangeX2
					    && touch.position.y > inputDetectionRangeY1 && touch.position.y < inputDetectionRangeY2) {
						
						float gestureTime = Time.time - fingerStartTime;
						float gestureDist = (touch.position - fingerStartPos).magnitude;

						// This is a swipe
						if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {

							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;
						
							if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
								// the swipe is horizontal:
								swipeType = Vector2.right * Mathf.Sign (direction.x);
							} else {
								// the swipe is vertical:
								swipeType = Vector2.up * Mathf.Sign (direction.y);
							}
						
							if (swipeType.x != 0.0f) {
								if (swipeType.x > 0.0f) {
//								Debug.Log ("SWIPE RIGHT");
									SWIPE_RIGHT = true;
								} else {
//								Debug.Log ("SWIPE LEFT");
									SWIPE_LEFT = true;
								}
							}
							
							if (swipeType.y != 0.0f) {
								if (swipeType.y > 0.0f) {
//								Debug.Log ("SWIPE UP");
									SWIPE_UP = true;
								} else {
//								Debug.Log ("SWIPE DOWN");
									SWIPE_DOWN = true;
								}
							}
						
						} else {
							TAP = true;
						}
					}
					break;
				}
			}
		}
	}
}