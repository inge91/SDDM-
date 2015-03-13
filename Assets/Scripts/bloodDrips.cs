using UnityEngine;
using System.Collections;

public class bloodDrips : MonoBehaviour {

	public Material mat;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination, mat);
	}
}
