using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
	public static ScreenFader fader;
	public Image FadeImg;
	public float fadeSpeed = 1.5f;

	private bool sceneStarting = true;
	private bool sceneEnding = false;
	
	
	void Awake()
	{
		fader = this;
		if(FadeImg == null) FadeImg = GetComponent<Image>();
		FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
	}
	
	void Update()
	{
		// If the scene is starting...
		if (sceneStarting)
			// ... call the StartScene function.
			StartSceneFadeOut();

		if (sceneEnding) {
			FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
			fadeSpeed += 0.05f;
		}
	}

	
	public void FadeToBlack()
	{
		// Lerp the colour of the image between itself and black.
		FadeImg.enabled = true;

		sceneEnding = true;
	}
	
	
	void StartSceneFadeOut()
	{
		// Fade the texture to clear.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
		fadeSpeed += 0.05f;
		
		// If the texture is almost clear...
		if (FadeImg.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the RawImage.
			FadeImg.color = Color.clear;
			FadeImg.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}

} 