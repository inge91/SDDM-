using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BulletSpawnerTrajectoryTest : MonoBehaviour
{
	public GameObject objectToSpawn;
	public GameObject targetObject;
	
	public AudioClip bulletSound;
	
	public float maxDistance;
	public float minDistance;
	
	public float maxSpeed;
	public float minSpeed;
	
	public float minIntervalBetweenProjectiles;
	public float maxIntervalBetweenProjectiles;

	public float maxAngle;
	public float errorThreshold;
	
	public bool projectileIs3D;
	
	private float timeSinceLastProjectile;
	private Vector3 targetPosition;

	private float lastDirection;
	private bool willHit = false;
	private bool hasClicked = false;
	
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
		if(Time.time - timeSinceLastProjectile > maxIntervalBetweenProjectiles)
		{
			timeSinceLastProjectile = Time.time;
			GameObject projectile = Instantiate(objectToSpawn);
			
			float radius = Random.Range(minDistance, maxDistance);
			float x, y, z;
			// start position should be on a circle around the user.  
			
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
			float deviation = Random.Range(0, 1);
			Vector3 projectileStartPosition = new Vector3(x, y, z);
			Vector3 direction = randomizedDirection(projectileStartPosition, targetPosition, deviation);

			lastDirection = direction;
			willHit = deviation < errorThreshold;
			hasClicked = false;

			projectile.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, maxSpeed, targetObject, csvWriter);
			projectile.AddComponent<AudioSource>();
			projectile.GetComponent<AudioSource>().clip = bulletSound;
			projectile.GetComponent<AudioSource>().playOnAwake = true;
			projectile.GetComponent<AudioSource>().loop = true;
			projectile.AddComponent<OSPAudioSource>();
		}

		if (Input.GetMouseButtonDown(0) && (!hasClicked))
		{
			csvWriter.writeLineToFile((Time.time - timeSinceLastProjectile) + ", " + "???" + ", " + willHit + ", " + willHit + ", " + lastDirection);
		}
		if (Input.GetMouseButtonDown(1) && (!hasClicked))
		{
			csvWriter.writeLineToFile((Time.time - timeSinceLastProjectile) + ", " + "???" + ", " + willHit + ", " + !willHit + ", " + lastDirection);
		}
	}

	Vector3 randomizedDirection(Vector3 startPosition, Vector3 targetPosition, float amount)
	{
		Vector3 forward = targetPosition - startPosition;
		float dist = forward.magnitude;

		float radius = Mathf.Tan(Mathf.Deg2Rad * maxAngle * amount) * dist;
		Vector2 circle = Random.insideUnitCircle * radius;
		Vector3 target = startPosition + forward + Quaternion.Euler(forward) * new Vector3(circle.x, circle.y);
		return Vector3.Normalize(target);
	}
}
