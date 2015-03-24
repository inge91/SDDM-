using UnityEngine;
using System.Collections;

public class ToggleCameras : MonoBehaviour {
	public Camera LeftEyeAnchor;
	public Camera RightEyeAnchor;
	public GameObject blackPlane;

	private bool CamerasOn = true;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("CameraToggle") && CamerasOn){
			blackPlane.gameObject.SetActive(true);
			//LeftEyeAnchor.gameObject.SetActive(false);
			//RightEyeAnchor.gameObject.SetActive(false);
			CamerasOn = false;
		}else if(Input.GetButtonDown ("CameraToggle")){
			blackPlane.gameObject.SetActive(false);
			//LeftEyeAnchor.gameObject.SetActive(true);
			//RightEyeAnchor.gameObject.SetActive(true);
			CamerasOn = true;
		}
	}


}
