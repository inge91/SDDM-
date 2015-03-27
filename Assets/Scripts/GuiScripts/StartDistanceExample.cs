using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StartDistanceExample : MonoBehaviour {

	public GameObject circles;
	public GameObject spawner;
	public GameObject canvas;
	public GameObject panelExample;
	public GameObject panelTest;
	public GameObject newSelectedObject;

	public void StartTest()
	{
		circles.SetActive (true);
		spawner.SetActive (true);
		panelExample.SetActive (false);
		panelTest.SetActive (true);
		EventSystem.current.SetSelectedGameObject (newSelectedObject);
		canvas.SetActive (false);
	}
}
