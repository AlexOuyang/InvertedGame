using UnityEngine;
using System.Collections;

//run the script in scene without press play
[ExecuteInEditMode]
public class ParticleSortLayerScript : MonoBehaviour
{
	public string sortingLayerName = "ObjectFront";

	void Start ()
	{
		//Change Foreground to the layer you want it to display on 
		GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingLayerName = sortingLayerName;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
