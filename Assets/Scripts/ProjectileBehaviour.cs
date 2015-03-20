using UnityEngine;
using System.Collections;

using AssemblyCSharp;

public class ProjectileBehaviour : MonoBehaviour {

	private Vector3 headingDirection;
	private float speed;
	private GameObject target;

	private Vector3 startingPosition;
	private float initialDistance;

	private CsvWriter csvWriter;

	public void Init(Vector3 startingPosition_, Vector3 headingDirection_, float speed_, GameObject target_, CsvWriter csvWriter_)
	{
		headingDirection = headingDirection_;
		speed = speed_;
		startingPosition = startingPosition_;
		target = target_;
		this.transform.position = startingPosition;
		initialDistance = Vector3.Distance (startingPosition, target.transform.position);
		csvWriter = csvWriter_;
	}

	// Update is called once per frame
	void Update () {
		this.transform.position += headingDirection * speed * Time.deltaTime;
		if (Vector3.Distance (startingPosition, this.transform.position) > initialDistance) {
			csvWriter.writeLineToFile(initialDistance.ToString() + ", " + speed.ToString() + ", " + startingPosition + ", " + "1");
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter (Collider c)
	{		
		if (c.gameObject.GetInstanceID () == target.GetInstanceID ()) {
			csvWriter.writeLineToFile(initialDistance.ToString() + ", " + speed.ToString() + ", " + startingPosition + ", " + "0");
			// Log succesful avoidance.
			Destroy (this.gameObject);
		}
	}
	
}
