using UnityEngine;
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
	public float[] possibleAngles;

	public float hitRange;
	
	public bool projectileIs3D;

	public int testCount;

	public int randomSeed;

	public bool isTutorial;

	private GameObject projectile;
	
	private float timeSinceLastProjectile;
	private Vector3 targetPosition;

	private Vector3 lastDirection;
	private bool hasClicked = false;

	private int currentTest = 0;
	
	private CsvWriter csvWriter;
	private float reactionTime;
	private float closestDistance;
	private bool guess;
	private float direction;
	
	// Use this for initialization
	void Start ()
	{
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;
		csvWriter = new CsvWriter("TrajectoryTest", "reactionTime;closestDist;hit;correct;direction");
		Random.seed = randomSeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!gameObject.activeSelf)
			return;
		if(Time.time - timeSinceLastProjectile > intervalBetweenProjectiles)
		{
			// Log experiment data.
			if (currentTest > 0)
			{
				bool hit = closestDistance < hitRange;
				bool correct = hasClicked && (hit == guess);
				string s = (hasClicked ? reactionTime.ToString() : "") + ";" + closestDistance + ";" + (hit ? "1" : "0") + ";" + (correct ? "1" : "0") + ";" + lastDirection;
				csvWriter.writeLineToFile(s);
				Debug.Log(s);
			}
			
			// Deactivate when tests are completed.
			currentTest++;
			if (currentTest > testCount)
			{
				gameObject.SetActive(false);
				Debug.Log("STOP");
				return;
			}

			// Create new bullet.
			timeSinceLastProjectile = Time.time;
			Destroy(projectile);
			projectile = Instantiate(objectToSpawn);

			// Calculate bullet position and direction.
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
			Vector3 projectileStartPosition = new Vector3(x, y, z);
			Vector3 direction = randomizedDirection(projectileStartPosition, targetPosition);

			lastDirection = direction;
			hasClicked = false;
			closestDistance = float.MaxValue;

			projectile.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, speed, targetObject, csvWriter);
			projectile.AddComponent<AudioSource>();
			projectile.GetComponent<AudioSource>().clip = bulletSound;
			projectile.GetComponent<AudioSource>().playOnAwake = true;
			projectile.GetComponent<AudioSource>().loop = true;
			projectile.AddComponent<OSPAudioSource>();
		}

		if (projectile != null) {
			Vector3 toProjectile = projectile.transform.position - targetPosition;
			closestDistance = Mathf.Min (closestDistance, toProjectile.magnitude);
		}

		bool controller = Input.GetJoystickNames ().Length > 0;

		if (((!controller && Input.GetMouseButtonDown(0)) || (controller && Input.GetButtonDown("JoystickButton0"))) && (!hasClicked))
		{
			reactionTime = Time.time - timeSinceLastProjectile;
			guess = true;
			hasClicked = true;
		}
		if (((!controller && Input.GetMouseButtonDown(1)) || (controller && Input.GetButtonDown("JoystickButton1"))) && (!hasClicked))
		{
			reactionTime = Time.time - timeSinceLastProjectile;
			guess = false;
			hasClicked = true;
		}
	}

	Vector3 randomizedDirection(Vector3 startPosition, Vector3 targetPosition)
	{
		float angle = possibleAngles[Random.Range(0, possibleAngles.Length)];
		float rotation = Random.Range (0, 30) * 12;

		Vector3 forward = targetPosition - startPosition;
		Vector3 tangent = Vector3.one;
		Vector3.OrthoNormalize (ref forward, ref tangent);

		Quaternion q1 = Quaternion.AngleAxis (rotation, forward);
		tangent = q1 * tangent;

		Quaternion q2 = Quaternion.AngleAxis (angle, tangent);
		return q2 * forward;
	}
}
