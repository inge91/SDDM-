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

	public float intervalTime;
	
	public bool playSoundRangeDemo = false;

	private float timeSinceLastProjectile;
	private Vector3 targetPosition;
	private GameObject ball;
	private CsvWriter csvWriter;
	private float radius = 0;

	private bool activeBallIsDemoBall;

	bool CR_Running = false;

	void Start () {
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;
		csvWriter = new CsvWriter ("AudioDistanceEstimateTest.txt", "ActualDistance, EstimateDistance, Error");

		if (playSoundRangeDemo) {
			StartCoroutine("spawnBallAfterInterval", true);
		} else {
			StartCoroutine("spawnBallAfterInterval", false);
		}
	}

	IEnumerator spawnBallAfterInterval(bool movingBall)
	{
		CR_Running = true;
		estimateCircle.DrawCircle(!movingBall);
		yield return new WaitForSeconds (intervalTime);
		spawnBall (movingBall);
		yield return null;
		CR_Running = false;
	}


	void Update () 
	{
		if(activeBallIsDemoBall && ball == null && !CR_Running)
		{	
			StartCoroutine("spawnBallAfterInterval", false);
		}

		if(Input.GetButtonDown("Fire1") && ! activeBallIsDemoBall)
		{
			// Get projectile in the scene, and its distance from target
			// Handle input selection	
			float estimate = estimateCircle.GetCurrentCircleRadius();
			float estimateError = Mathf.Abs(estimate - radius);
			csvWriter.writeLineToFile(radius + ", " + estimate +", "+ estimateError);	
			Destroy(ball);
			if (playSoundRangeDemo) {
				StartCoroutine("spawnBallAfterInterval", true);
			} else {
				StartCoroutine("spawnBallAfterInterval", false);
			}
		}
	}

	private void spawnBall(bool ballIsMoving)
	{
		activeBallIsDemoBall = ballIsMoving;

		ball = Instantiate (objectToSpawn);
		timeSinceLastProjectile = Time.time;
			
		// Demo ball should always start from the maxDistance and travel to the player position.
		if (ballIsMoving) {
			radius = maxDistance;
			float x,y,z;
			x = targetPosition.x + radius * Mathf.Cos(0);
			y = 0;
			z = targetPosition.y + radius * Mathf.Sin(0);
			Vector3 projectileStartPosition = new Vector3(x, y, z);
			Vector3 direction = Vector3.Normalize(targetPosition - projectileStartPosition);
			ball.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, 1, targetObject, csvWriter);
		} else {
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
		}

		// This should be handled in the projectile itself in my opinion.
		ball.AddComponent<AudioSource>();
		ball.GetComponent<AudioSource>().clip = bulletSound;
		ball.GetComponent<AudioSource>().playOnAwake = true;
		ball.GetComponent<AudioSource>().loop = true;
		ball.AddComponent<OSPAudioSource>();
	}

	
}
