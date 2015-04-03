using UnityEngine;
using System.Collections;

//Rotate the object to which the script is attachted towards a given target object
//Adjust the speed parameter to adjust the rotation speed
public class RotateTowards : MonoBehaviour {
	public GameObject target; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int speed = 1000;
		var q = Quaternion.LookRotation(target.transform.position - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);
	}
}
