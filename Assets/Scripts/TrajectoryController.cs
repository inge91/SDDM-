using UnityEngine;
using System.Collections;

public class TrajectoryController : MonoBehaviour {

	public GameObject bulletSpawner;
	public GameObject bulletSpawnerTutorial;

	// Use this for initialization
	void Start () {
		bulletSpawner.SetActive (false);
		bulletSpawnerTutorial.SetActive (true);//
	}
	
	// Update is called once per frame
	void Update () {
		// TODO:
		// 1. Activate UI.
		// 2. After button press, activate tutorial.
		// 3. After tutorial stops, activate UI.
		// 4. After button press, activate actual experiment.
	}
}
