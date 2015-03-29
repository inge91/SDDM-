﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BulletSpawnerTrajectoryTest : MonoBehaviour
{
	public GameObject objectToSpawn;
	public GameObject targetObject;
	
	public AudioClip bulletSound;
	
	public float distance;
	
	public float speed;
	
	public float intervalBetweenProjectiles;

	public float maxAngle;
	public float errorThreshold;
	
	public bool projectileIs3D;

	private GameObject projectile;
	
	private float timeSinceLastProjectile;
	private Vector3 targetPosition;

	private Vector3 lastDirection;
	private bool willHit = false;
	private bool hasClicked = true;
	
	private CsvWriter csvWriter;
	
	// Use this for initialization
	void Start ()
	{
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;
		csvWriter = new CsvWriter("TrajectoryTest.txt", "reactionTime, closestDist, hit, correct, direction");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.time - timeSinceLastProjectile > intervalBetweenProjectiles)
		{
			if (!hasClicked)
			{
				csvWriter.writeLineToFile("-" + ", " + "???" + ", " + (willHit ? "yes" : "no") + ", " + "no" + ", " + lastDirection);
			}
			timeSinceLastProjectile = Time.time;
			Destroy(projectile);
			projectile = Instantiate(objectToSpawn);
			
			float radius = distance;
			float x, y, z; 
			
			if (projectileIs3D)
			{
				float radiansX = Random.Range (0, Mathf.PI * 2);
				// Minimum y radians is not 0 so that the projectiles cannot originate from under the player.
				float radiansY = Random.Range (Mathf.PI / 2, Mathf.PI * 2);
				x = targetPosition.x + radius * Mathf.Cos(radiansX) * Mathf.Sin(radiansY);
				y = targetPosition.y + radius * Mathf.Sin(radiansX) * Mathf.Sin(radiansY);
				z = targetPosition.z + radius * Mathf.Cos(radiansY);
			}
			else
			{
				float radians = Random.Range (0, Mathf.PI * 2);
				x = targetPosition.x + radius * Mathf.Cos(radians);
				y = 0;
				z = targetPosition.y + radius * Mathf.Sin(radians);
			}
			float deviation = Random.Range(0f, 1f);
			Vector3 projectileStartPosition = new Vector3(x, y, z);
			Vector3 direction = randomizedDirection(projectileStartPosition, targetPosition, deviation);

			lastDirection = direction;
			willHit = deviation < errorThreshold;
			hasClicked = false;

			projectile.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, speed, targetObject, csvWriter);
			projectile.AddComponent<AudioSource>();
			projectile.GetComponent<AudioSource>().clip = bulletSound;
			projectile.GetComponent<AudioSource>().playOnAwake = true;
			projectile.GetComponent<AudioSource>().loop = true;
			projectile.AddComponent<OSPAudioSource>();
		}

		if (Input.GetMouseButtonDown(0) && (!hasClicked))
		{
			csvWriter.writeLineToFile((Time.time - timeSinceLastProjectile) + ", " + "???" + ", " + (willHit ? "yes" : "no") + ", " + (willHit ? "yes" : "no") + ", " + lastDirection);
			hasClicked = true;
		}
		if (Input.GetMouseButtonDown(1) && (!hasClicked))
		{
			csvWriter.writeLineToFile((Time.time - timeSinceLastProjectile) + ", " + "???" + ", " + (willHit ? "yes" : "no") + ", " + (!willHit ? "yes" : "no") + ", " + lastDirection);
			hasClicked = true;
		}
	}

	Vector3 randomizedDirection(Vector3 startPosition, Vector3 targetPosition, float deviation)
	{
		Debug.Log(deviation * maxAngle);

		Vector3 forward = targetPosition - startPosition;
		Vector3 tangent = Vector3.one;
		Vector3.OrthoNormalize (ref forward, ref tangent);

		float rotation = Random.Range (0, 360);
		Quaternion q = Quaternion.AngleAxis(rotation, forward);
		tangent = q * tangent;

		Vector3 binormal = Vector3.Normalize (Vector3.Cross(forward, tangent));

		float s = Random.Range (0f, 1f);
		float r = Random.Range (0f, 1f);
		float h = Mathf.Cos (Mathf.Deg2Rad * deviation * maxAngle);

		float phi = 2 * Mathf.PI * s;
		float z = h + (1 - h) * r;
		float sinT = Mathf.Sqrt (1 - z * z);
		float x = Mathf.Cos (phi) * sinT;
		float y = Mathf.Sin (phi) * sinT;

		return tangent * x + binormal * y + forward * z;
	}
}
