using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StartDirectionExample : MonoBehaviour {

	public GameObject exampleSpawner;
    public GameObject spawner;
	public GameObject panelExample;
	public GameObject panelTest;

    public GameObject multiExampleSpawner;
    public GameObject panelExampleMulti;
    public GameObject panelTestMulti;

    public GameObject canvas;

	public void StartExample()
	{
		exampleSpawner.SetActive (true);
		panelExample.SetActive (false);
        panelTest.SetActive (true);
        canvas.SetActive (false);
	}

    public void StartExampleMulti()
    {
        spawner.SetActive (false);
        multiExampleSpawner.SetActive (true);
        panelExampleMulti.SetActive (false);
        panelTestMulti.SetActive (true);
        canvas.SetActive (false);
    }
}
