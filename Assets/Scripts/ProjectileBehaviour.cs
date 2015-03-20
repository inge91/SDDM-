using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {

	private Vector3 headingDirection;
	private float speed;
	private GameObject target;

	private Vector3 startingPosition;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnCollsionEnter(Collision c)
	{
		if (c.gameObject.GetInstanceID () == target.GetInstanceID ()) {
			Destroy (this);
		}
	}


}
