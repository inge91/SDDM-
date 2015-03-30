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

    public GameObject nextButton; // button from direction example to direction test
    public GameObject nextButton2; //button from direction test to multi direction example

    public GameObject canvas;

	public void StartExample()
	{
		exampleSpawner.SetActive (true);
		panelExample.SetActive (false);
        panelTest.SetActive (true);
        EventSystem.current.SetSelectedGameObject(nextButton);
        canvas.SetActive (false);

	}

    public void StartExampleMulti()
    {
        spawner.SetActive (false);
        multiExampleSpawner.SetActive (true);
        panelExampleMulti.SetActive (false);
        panelTestMulti.SetActive (true);
        EventSystem.current.SetSelectedGameObject(nextButton2);
        canvas.SetActive (false);
    }
}
