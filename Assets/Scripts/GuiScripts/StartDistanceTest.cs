using UnityEngine;
using System.Collections;

public class StartDistanceTest: MonoBehaviour {
	public GameObject circles;
	public GameObject spawner;
	public GameObject canvas;

	public void StartTest()
	{
		circles.SetActive (true);
		spawner.SetActive (true);
		canvas.SetActive (false);
	}
}
