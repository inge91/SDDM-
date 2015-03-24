using UnityEngine;
using System.Collections;

public class SoundOnCollision : MonoBehaviour {
	public string bulletName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.transform.root.name == bulletName){
			this.GetComponent<AudioSource>().Play();
			Destroy(collider.gameObject);
		}

	}
}
