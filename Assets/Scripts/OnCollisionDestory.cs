using UnityEngine;
using System.Collections;

public class OnCollisionDestory : MonoBehaviour {
	public string bulletName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit(Collider collider) {
		if(collider.gameObject.transform.root.name == bulletName){
			Destroy(collider.gameObject);
		}
	}
}
