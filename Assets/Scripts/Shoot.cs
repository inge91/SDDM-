using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    public float speed = 2;

    public float fireRate;
    public Transform shotSpawn;
    public GameObject shot;

    private float nextFire;

	// Use this for initialization
	void Start () {
        speed = 2;
        fireRate = 10;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
	
	}
}
