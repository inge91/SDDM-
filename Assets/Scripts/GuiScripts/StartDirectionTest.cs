using UnityEngine;
using System.Collections;

public class StartDirectionTest: MonoBehaviour {

    public GameObject exampleSpawner;
    public GameObject spawner;
    public GameObject panelTest;

    public GameObject multiExampleSpawner;
    public GameObject multiSpawner;
    public GameObject panelExampleMulti;
    public GameObject panelTestMulti;

	public GameObject canvas;


	public void StartTest()
	{
        exampleSpawner.SetActive(false);
		spawner.SetActive (true);
        panelTest.SetActive (false);
        panelExampleMulti.SetActive(true);
		canvas.SetActive (false);
	}

    public void StartTestMulti()
    {
        multiExampleSpawner.SetActive(false);
        multiSpawner.SetActive(true);
        panelTestMulti.SetActive(false);
        canvas.SetActive(false);
    }
}
