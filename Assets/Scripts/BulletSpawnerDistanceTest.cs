using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;


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

	public GameObject gui;


	public bool useDiscreteInputToTest;
	// These variables are only used in case useDiscreteInputToTest is set to true.
	// partitionsToTest divides up the circle in equal pieces and tests on exactly those positions.
	public int partitionsToTest;
	public float[] distancesToTest;

	public bool isExample = false;

	private List<DistancePartitionStruct> discreteInputList;
	
	bool CR_Running = false;
	void Start () {
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;

		if (!isExample) {
			csvWriter = new CsvWriter ("AudioDistanceEstimateTest.txt", "ActualDistance, EstimateDistance, Error");
		};
		if (playSoundRangeDemo) {
			StartCoroutine("spawnBallAfterInterval", true);
		} else {
			StartCoroutine("spawnBallAfterInterval", false);
		}

		discreteInputList = new  List<DistancePartitionStruct>();
		float radians = (2 * Mathf.PI) / partitionsToTest; 
		for(int i = 0; i < partitionsToTest; i ++)
		{
			for(int j = 0; j < distancesToTest.Length; j ++)
			{
				DistancePartitionStruct s = new DistancePartitionStruct(radians * i, distancesToTest[j]);
				discreteInputList.Add(s);
			}
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

	bool testIsOver()
	{
		if (discreteInputList.Count == 0 && useDiscreteInputToTest) {
			return true;
		}
		return false;
	}

	void Update () 
	{
		if(activeBallIsDemoBall && ball == null && !CR_Running)
		{	
			StartCoroutine("spawnBallAfterInterval", false);
		}

		if(Input.GetButtonDown("Fire1") && ! activeBallIsDemoBall && !destroyingObjectDelay && ball != null)
		{
			// Get projectile in the scene, and its distance from target
			// Handle input selection	
			float estimate = estimateCircle.GetCurrentCircleRadius();
			float estimateError = Mathf.Abs(estimate - radius);
			if(!isExample)
			{
				csvWriter.writeLineToFile(radius + ", " + estimate +", "+ estimateError);	
			}
			if(isExample)
			{
				ball.GetComponent<ProjectileBehaviour> ().SetVisible (true);
				StartCoroutine("DestroyObjectAfterTime", ball);
			}
			else{
				Destroy(ball);
			}
			if (playSoundRangeDemo && !testIsOver()) {
				StartCoroutine("spawnBallAfterInterval", true);
			} else {
				StartCoroutine("spawnBallAfterInterval", false);
			}
		}
	}

	bool destroyingObjectDelay = false;
	IEnumerator DestroyObjectAfterTime(GameObject objectToDestroy)
	{
		destroyingObjectDelay = true;
		yield return new WaitForSeconds (intervalTime);
		Destroy (objectToDestroy);
		yield return null;
		destroyingObjectDelay = false;
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
			x = targetPosition.x + radius * Mathf.Cos(Mathf.PI/2);
			y = 0;
			z = targetPosition.y + radius * Mathf.Sin(Mathf.PI/2);
			Vector3 projectileStartPosition = new Vector3(x, y, z);
			Vector3 direction = Vector3.Normalize(targetPosition - projectileStartPosition);
			ball.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, 1, targetObject, csvWriter);
		} else {

			float x,y,z;
			float radians = 0;
			if(useDiscreteInputToTest)
			{
				if(discreteInputList.Count == 0)
				{
					if(isExample)
					{
						Destroy(ball);
						StartCoroutine("DestroyObjectAfterTime", this.gameObject);
						gui.SetActive(true);
					}
					else{
					}
				}
				else
				{
					int indexToTest = Random.Range(0, discreteInputList.Count);
					DistancePartitionStruct tuple = discreteInputList[indexToTest];
					radius = tuple.distance;
					radians = tuple.degrees;
					discreteInputList.RemoveAt(indexToTest);
					x = targetPosition.x + radius * Mathf.Cos(radians);
					y = 0;
					z = targetPosition.y + radius * Mathf.Sin(radians);
					
					Vector3 projectileStartPosition = new Vector3(x, y, z);
					ball.GetComponent<ProjectileBehaviour> ().Init (projectileStartPosition, new Vector3 (0, 0, 0), 0, targetObject, csvWriter);
					ball.GetComponent<ProjectileBehaviour> ().SetVisible (false);
				}
			}
			else{
				radius = Random.Range(minDistance, maxDistance);
				// start position should be on a circle around the user.  
				radians = Random.Range (0, Mathf.PI * 2);
				x = targetPosition.x + radius * Mathf.Cos(radians);
				y = 0;
				z = targetPosition.y + radius * Mathf.Sin(radians);
				
				Vector3 projectileStartPosition = new Vector3(x, y, z);
				ball.GetComponent<ProjectileBehaviour> ().Init (projectileStartPosition, new Vector3 (0, 0, 0), 0, targetObject, csvWriter);
				ball.GetComponent<ProjectileBehaviour> ().SetVisible (false);
			
			}
		}
		// This should be handled in the projectile itself in my opinion.
		ball.AddComponent<AudioSource>();
		ball.GetComponent<AudioSource>().clip = bulletSound;
		ball.GetComponent<AudioSource>().playOnAwake = true;
		ball.GetComponent<AudioSource>().loop = true;
		ball.AddComponent<OSPAudioSource>();

	
	}
}
