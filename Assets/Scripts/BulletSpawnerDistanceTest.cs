using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class BulletSpawnerDistanceTest : MonoBehaviour {

	public GameObject objectToSpawn;
	public GameObject targetObject;
	public CircleDraw estimateCircle;

	public AudioClip bulletSound;

	public float maxDistance;
	public float minDistance;

	private float timeSinceLastProjectile;
	private Vector3 targetPosition;
	private GameObject ball;
	private CsvWriter csvWriter;
	private float radius = 0;
	void Start () {
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;
		csvWriter = new CsvWriter ("AudioDistanceEstimateTest.txt", "ActualDistance, EstimateDistance, Error");
		spawnBall();
	}


	void Update () {
		
		if(Input.GetButtonDown("Fire1"))
		{
			// Get projectile in the scene, and its distance from  targe
			// Handle input selection	
			float estimate = estimateCircle.GetCurrentCircleRadius();
			float estimateError = Mathf.Abs(estimate - radius);
			csvWriter.writeLineToFile(radius + ", " + estimate +", "+ estimateError);	
			Destroy(ball);
			spawnBall();
		}
	}


	private void spawnBall()
	{
		ball = Instantiate (objectToSpawn);
		timeSinceLastProjectile = Time.time;
		GameObject projectile = Instantiate(objectToSpawn);
			
		radius = Random.Range(minDistance, maxDistance);
		float x,y,z;

		// start position should be on a circle around the user.  
		float radians = Random.Range (0, Mathf.PI * 2);
		x = targetPosition.x + radius * Mathf.Cos(radians);
		y = 0;
		z = targetPosition.y + radius * Mathf.Sin(radians);

		Vector3 projectileStartPosition = new Vector3(x, y, z);

		ball.GetComponent<ProjectileBehaviour> ().Init (projectileStartPosition, new Vector3 (0, 0, 0), 0, targetObject, csvWriter);
		ball.GetComponent<ProjectileBehaviour> ().SetVisible (false);
		// This should be handled in the projectile itself in my opinion.
		ball.AddComponent<AudioSource>();
		ball.GetComponent<AudioSource>().clip = bulletSound;
		ball.GetComponent<AudioSource>().playOnAwake = true;
		ball.GetComponent<AudioSource>().loop = true;
		ball.AddComponent<OSPAudioSource>();
	
	}

	
}
