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
			csvWriter = new CsvWriter ("AudioDistanceEstimateTest", "ProjectilePosition, ActualDistance; EstimateDistance; Error");
		};
		if (playSoundRangeDemo) {
			StartCoroutine("spawnBallAfterInterval", true);
		} else {
			StartCoroutine("spawnBallAfterInterval", false);
		}

		discreteInputList = new  List<DistancePartitionStruct>(); 
		if (isExample) {
			DistancePartitionStruct s = new DistancePartitionStruct(0, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(0, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(0, 6);
			discreteInputList.Add(s);
		} else {
			float partition = (Mathf.PI * 2) / 8.0f;
			DistancePartitionStruct s = new DistancePartitionStruct(partition * 0, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 5, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 2, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 6, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 4, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 1, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 3, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 8, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 7, 8);
			discreteInputList.Add(s);

			s = new DistancePartitionStruct(partition * 3, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 6, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 8, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 4, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 7, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 1, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 2, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 0, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 5, 8);
			discreteInputList.Add(s);

			s = new DistancePartitionStruct(partition * 2, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 7, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 0, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 3, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 4, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 8, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 5, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 1, 6);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 6, 4);
			discreteInputList.Add(s);
	
			s = new DistancePartitionStruct(partition * 3, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 6, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 8, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 4, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 7, 4);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 1, 8);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 2, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 0, 2);
			discreteInputList.Add(s);
			s = new DistancePartitionStruct(partition * 5, 4);
			discreteInputList.Add(s);

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

				csvWriter.writeLineToFile(  ball.transform.position + "; " +  radius + "; " + estimate + "; " + estimateError);	
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
	Vector3 projectileStartPosition ;
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
			ball.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, 1.5f, targetObject, csvWriter);
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
						csvWriter.Close();
                        Application.Quit();
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
				
				projectileStartPosition = new Vector3(x, y, z);
				ball.GetComponent<ProjectileBehaviour> ().Init (projectileStartPosition, new Vector3 (0, 0, 0), 0, targetObject, csvWriter);
				ball.GetComponent<ProjectileBehaviour> ().SetVisible (false);
			
			}
		}
		ball.transform.localScale = new Vector3 (.5f, .5f, .5f);
		// This should be handled in the projectile itself in my opinion.
		ball.AddComponent<AudioSource>();
		ball.GetComponent<AudioSource>().clip = bulletSound;
		ball.GetComponent<AudioSource>().playOnAwake = true;
		ball.GetComponent<AudioSource>().loop = true;
		ball.AddComponent<OSPAudioSource>();

	
	}
}
