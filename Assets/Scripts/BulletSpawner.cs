using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour {

	public GameObject objectToSpawn;
	public GameObject targetObject;

	public float maxDistance;
	public float minDistance;

	public float maxSpeed;
	public float minSpeed;

	public float minIntervalBetweenProjectiles;
	public float maxIntervalBetweenProjectiles;

	public bool projectileIs3D;

	private float timeSinceLastProjectile;
	private Vector3 targetPosition;

	void Start () {
		timeSinceLastProjectile = Time.time;
		targetPosition = targetObject.transform.position;
	}
	

	void Update () {
		if(Time.time - timeSinceLastProjectile > maxIntervalBetweenProjectiles)
		{

			timeSinceLastProjectile = Time.time;
			GameObject projectile = Instantiate(objectToSpawn);

			float radius = Random.Range(minDistance, maxDistance);

			// start position should be on a circle around the user.  
			float radians = Random.Range (0, Mathf.PI * 2);
			float x = targetPosition.x + radius * Mathf.Cos(radians);
			float y = 0;
			float z = targetPosition.y + radius * Mathf.Sin(radians);

			projectile.transform.position = new Vector3(x, y, z);

		}
	}
}
