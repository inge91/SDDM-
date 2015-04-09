using UnityEngine;
using System.Collections;

public class SoundOnCollision : MonoBehaviour {
	public string bulletName;
    private BeltRumble rumble;
	public bool isPlayerObject = false; //set this to true if the script is attached to the player
	public GameObject sceneManager;

	// Use this for initialization
	void Start () {
        GameObject temp = GameObject.Find("RumbleObject");
        rumble = (BeltRumble)temp.GetComponent(typeof(BeltRumble));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.transform.root.name == bulletName){
            rumble.PlayerHit(collider.gameObject.transform.position);

			this.GetComponent<AudioSource>().Play();
			Destroy(collider.gameObject);

			//remove one health from the player
			sceneManager.GetComponent<GameManager>().health -= 1;
		}

	}
}
