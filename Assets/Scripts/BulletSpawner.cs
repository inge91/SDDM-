using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class BulletSpawner : MonoBehaviour {

	public GameObject objectToSpawn;
	public GameObject targetObject;

	public AudioClip bulletSound;

	public float maxDistance;
	public float minDistance;

	public float maxSpeed;
	public float minSpeed;

	public float minIntervalBetweenProjectiles;
	public float maxIntervalBetweenProjectiles;

	//TODO: Use this as well
	public bool projectileIs3D;

	private float timeSinceLastProjectile;
	private Vector3 targetPosition;

	private CsvWriter csvWriter;

	void Start () {
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;
		csvWriter = new CsvWriter ("test.txt", "startingDistance, speed, startingPosition, avoided");
	}
	

	void Update () {
		if(Time.time - timeSinceLastProjectile > maxIntervalBetweenProjectiles)
		{

			timeSinceLastProjectile = Time.time;
			GameObject projectile = Instantiate(objectToSpawn);

			float radius = Random.Range(minDistance, maxDistance);
			float x,y,z;
			// start position should be on a circle around the user.  

			if(projectileIs3D)
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
			Vector3 direction = Vector3.Normalize(targetPosition - projectileStartPosition);
			projectile.GetComponent<ProjectileBehaviour>().Init(projectileStartPosition, direction, maxSpeed, targetObject, csvWriter);
			projectile.AddComponent<AudioSource>();
			projectile.GetComponent<AudioSource>().clip = bulletSound;
			projectile.GetComponent<AudioSource>().playOnAwake = true;
			projectile.GetComponent<AudioSource>().loop = true;
			projectile.AddComponent<OSPAudioSource>();

		}
	}
}
