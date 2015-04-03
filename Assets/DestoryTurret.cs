using UnityEngine;
using System.Collections;

public class DestoryTurret : MonoBehaviour {
	public string bulletName;
	public float respawnTime = 3f;
	public GameObject turret;
	public GameObject turretParent;

	private float nextTurret = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextTurret && !turret.activeInHierarchy) {
			//enable turret
			turret.SetActive(true);
			this.GetComponent<BoxCollider>().enabled = true;
		}
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.transform.root.name == bulletName){
			this.GetComponent<AudioSource>().Play();
			nextTurret = Time.time + respawnTime;

			//disable turret
			turret.SetActive(false);
			this.GetComponent<BoxCollider>().enabled = false;


			//Move the turret to a new random position

			turretParent.gameObject.transform.rotation = Quaternion.Euler(0, Random.Range(-90.0f, 90.0f), 0);
		}
		
	}
}
