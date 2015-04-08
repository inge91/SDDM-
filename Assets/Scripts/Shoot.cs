using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    public float speed = 2;
	public bool autoFire = false;
	public bool rumbleOn = false;

    public bool useChargingSound;

    public AudioClip chargingSound;

    public float fireRate;
    public Transform shotSpawn;
    public GameObject shot;

    private ControllerRumble rumble;

    private float nextFire;
    private float chargeTime;
    private float nextCharge;

	// Use this for initialization
	void Start () {
        //speed = 2;
        //fireRate = 1;

        nextFire = Time.time + fireRate;
        if (chargingSound != null)
        {
            chargeTime = chargingSound.length;
            nextFire = nextFire + chargeTime;
        }

        nextCharge = Time.time + fireRate;


        GameObject temp = GameObject.Find("RumbleObject");
        rumble = (ControllerRumble)temp.GetComponent(typeof(ControllerRumble));
	}
	
	// Update is called once per frame
	void Update () {
		if(autoFire){
            if (useChargingSound && Time.time > nextCharge)
            {
                
                AudioSource.PlayClipAtPoint(chargingSound, this.transform.position);
                nextCharge = Time.time + fireRate;
            }
			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				if(rumbleOn){
					rumble.PlayerShoot();
				}
			}
		}else if(Input.GetMouseButtonDown(0) || Input.GetKeyDown("space") || (Input.GetButtonDown("JoystickButton0"))) {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			if(rumbleOn){
				rumble.PlayerShoot();
			}
           
        }
	
	}
}
