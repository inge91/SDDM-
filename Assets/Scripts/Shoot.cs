using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    public float speed = 2;
	public float cooldown = 3;
	public bool autoFire = false;
	public bool rumbleOn = false;

	public bool useChargingSound = false;
	public AudioClip overrideBulletSound;

    //public AudioClip chargingSound;

    public float fireRate;
    public Transform shotSpawn;
    public GameObject shot;

    private ControllerRumble rumble;

    private float nextFire;
    private float chargeTime;
    private float nextCharge;
	private float cooldownDone = 0;

	// Use this for initialization
	void Start () {
        //speed = 2;
        //fireRate = 1;

        nextFire = Time.time + fireRate;

		if(useChargingSound){
			chargeTime = this.gameObject.GetComponent<AudioSource>().clip.length;
			nextFire = Time.time + fireRate + chargeTime*0.8f;
			nextCharge = Time.time + fireRate;
		}
		
        GameObject temp = GameObject.Find("RumbleObject");
        rumble = (ControllerRumble)temp.GetComponent(typeof(ControllerRumble));
	}
	
	// Update is called once per frame
	void Update () {
		if(autoFire){
            if (useChargingSound && Time.time > nextCharge)
            {
                
                //AudioSource.PlayClipAtPoint(chargingSound, this.transform.position);
				this.gameObject.GetComponent<AudioSource>().Play();
                nextCharge = Time.time + fireRate;
            
			if (Time.time > nextFire) {
				nextFire = nextCharge + chargeTime*0.8f;

				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				if(rumbleOn){
					rumble.PlayerShoot();
				}
			}
			}else{
				if (Time.time > nextFire) {
					nextFire = Time.time + fireRate;

					shot.GetComponent<AudioSource>().clip = overrideBulletSound;
					Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
					if(rumbleOn){
						rumble.PlayerShoot();
					}
				}
			}
		}else if(Input.GetMouseButtonDown(0) || (Input.GetButtonDown("JoystickButton0")) || (Input.GetAxis("Right Trigger") == 1)) {
			if(Time.time > cooldownDone){
				cooldownDone = Time.time + cooldown;
				//nextFire = Time.time + fireRate;
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				if(rumbleOn){
					rumble.PlayerShoot();
				}
			}

           
        }
	
	}
}
