using UnityEngine;
using System.Collections;

public class ToggleCameras : MonoBehaviour {
	public Camera LeftEyeAnchor;
	public Camera RightEyeAnchor;
	public GameObject blackPlane;

	private bool CamerasOn = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("CameraToggle") && CamerasOn){
			blackPlane.gameObject.SetActive(true);
			//LeftEyeAnchor.gameObject.SetActive(false);
			//RightEyeAnchor.gameObject.SetActive(false);
			LeftEyeAnchor.nearClipPlane = 100;
			RightEyeAnchor.nearClipPlane = 100;
			CamerasOn = false;
		}else if(Input.GetButtonDown ("CameraToggle")){
			blackPlane.gameObject.SetActive(false);
			//LeftEyeAnchor.gameObject.SetActive(true);
			//RightEyeAnchor.gameObject.SetActive(true);
			LeftEyeAnchor.nearClipPlane = 0.1f;
			RightEyeAnchor.nearClipPlane = 0.1f;
			CamerasOn = true;
		}
	}


}
