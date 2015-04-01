using UnityEngine;
using System.Collections;

public class TrajectoryController : MonoBehaviour {

	public GameObject bulletSpawner;
	public GameObject bulletSpawnerTutorial;
	public GameObject tutorialPanel;
	public GameObject testPanel;
	public GameObject finishPanel;

	private int progress = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			switch (progress){
			case 0:
				startTutorial();
				break;
			case 2:
				startTest ();
				break;
			}
		}

		if ((progress == 1) && !bulletSpawnerTutorial.activeSelf)
			endTutorial ();
		if ((progress == 3) && !bulletSpawner.activeSelf)
			endTest ();
	}

	public void startTutorial()
	{
		Destroy (tutorialPanel);
		bulletSpawnerTutorial.SetActive (true);
		progress++;
	}
	
	public void endTutorial()
	{
		Destroy (bulletSpawnerTutorial);
		testPanel.SetActive (true);
		progress++;
	}
	
	public void startTest()
	{
		Destroy (testPanel);
		bulletSpawner.SetActive (true);
		progress++;
	}
	
	public void endTest()
	{
		Destroy (bulletSpawner);
		finishPanel.SetActive (true);
		progress++;
	}
}
