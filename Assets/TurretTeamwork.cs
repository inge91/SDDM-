using UnityEngine;
using System.Collections;

public class TurretTeamwork : MonoBehaviour {
	public GameObject otherTurret; //other turret in the scene
	public GameObject turretBody; //body of this turret

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//If the turret is waiting to respawn
		//and the new location is too close to the other turrets location
		//randomly pick a new location
		if((Mathf.Abs(this.gameObject.transform.rotation.y - otherTurret.transform.rotation.y) < 20) && !turretBody.activeInHierarchy){
			this.gameObject.transform.rotation = Quaternion.Euler(0, Random.Range(-90.0f, 90.0f), 0);
		}
	}
}
