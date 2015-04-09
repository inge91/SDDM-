using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int health = 5;
	public int score = 0;
	public GameObject turretParent;
	public GameObject secondTurretParent;
	public GameObject startGame;
	public GameObject gameOver;
	public GameObject blackPlane; //The plane infront of the cameras

	public Camera LeftEyeAnchor;
	public Camera RightEyeAnchor;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Reset")){
			OVRManager.display.RecenterPose();
		}
		if(Input.GetButtonDown("Startgame")){
			startGame.SetActive(false);
			turretParent.SetActive(true);
		}
		if(Input.GetButtonDown("Restart")){
			Application.LoadLevel(0);
		}
		if(Input.GetButtonDown("InfLife")){
			health = -1;
		}
		if(Input.GetButtonDown("SpawnSecTurret")){
			secondTurretParent.SetActive(true);
		}
		if(Input.GetButtonDown("SwitchLevel")){
			Application.LoadLevel(1);
		}


		if(health == 0){
			gameOver.SetActive(true);
			gameOver.GetComponent<TextMesh>().text = string.Concat("Score: ", score); 
			turretParent.SetActive(false);
			blackPlane.SetActive(false);
			LeftEyeAnchor.nearClipPlane = 0.1f;
			RightEyeAnchor.nearClipPlane = 0.1f;
		}
	}
}
