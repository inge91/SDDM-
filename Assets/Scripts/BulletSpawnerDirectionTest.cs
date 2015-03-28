using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;


public class BulletSpawnerDirectionTest : MonoBehaviour
{

    // public variables set in unity scene inspector?
    public GameObject objectToSpawn;  //used to generate new objects making sounds
    public GameObject playerObject;
    public float radius;            // constant value of how far away objects are
    public bool directionOnlyFront; // don't place sounds behind the player
    public bool directionIs3D;      //place sound on 2d circle or 3d sphere
    public float heightLimit = 0.125f; // don't use the upper and lower 1/8


    public float startDelay;        // wait a little before starting the spawning etc
    public int amountOfSounds;      //amount of sound emitting objects spawned
    public bool uniqueSounds;       // do they play unique sounds
    public float audioSpawnDelay;  //delay inbetween spawns
    public List<AudioClip> audioClips;


    

    // private variables in code?
    private List<GameObject> balls;
    private Vector3 playerPosition;
    private List<Vector3> guesses;
    private CsvWriter csvWriter;
    private bool spawning = false;


    void Start()
    {
        playerPosition = playerObject.transform.position;
        guesses = new List<Vector3>();
        /*csvWriter = new CsvWriter("AudioDirectionEstimateTest.txt", "Object number, Actual angle, EstimateAngle, Error");*/
        balls = new List<GameObject>();

        StartCoroutine(spawnBallsAfterInterval());
    }

    IEnumerator spawnBallsAfterInterval()
    {
        spawning = true;
        int amount = amountOfSounds;
        if(uniqueSounds)
            amount = audioClips.Count < amount ? audioClips.Count : amount;

        yield return new WaitForSeconds(startDelay);
        for (int i = 0; i < amount; i++)
        {
            if (i > 0 && audioSpawnDelay > 0)
            {
                yield return new WaitForSeconds(audioSpawnDelay);
                spawnBall(i);
            }
            else
                spawnBall(i);
        }
        spawning = false;
        yield return null;
    }


    void Update()
    {
        
        //press button after listening
        if(Input.GetButtonDown("Fire1") && !spawning)
		{
            //get oculus look direction
            var centerEyeAnchor = playerObject.transform.GetChild(1);
            Vector3 lookForward = centerEyeAnchor.forward;
            if (!directionIs3D)
                lookForward.y = 0;
            float angleDegree = Vector3.Angle(Vector3.forward, lookForward);

            // find closest ball & determine error
        }

    }



    private void spawnBall(int id)
    {
        GameObject ball = Instantiate(objectToSpawn);

        // start position should be on a circle/sphere around the user.  
        float x, z, y, radiansX= 0, radiansY = 0;
        //radX 0 -> right, radx pi -> left
        //radY 0 -> top, radY pi -> bottom

        if (directionOnlyFront)
            radiansX = Random.Range(0, Mathf.PI);
        else
            radiansX = Random.Range(0, Mathf.PI * 2);
        if (directionIs3D)
        {
            // radiansY has heightLimit to prevent placement exactly below/above the player
            radiansY =  Random.Range(heightLimit * Mathf.PI, (1.0f - heightLimit) * Mathf.PI); 
            x = playerPosition.x + radius * Mathf.Cos(radiansX) * Mathf.Sin(radiansY);
            y = playerPosition.z + radius * Mathf.Cos(radiansY);
            z = playerPosition.y + radius * Mathf.Sin(radiansX) * Mathf.Sin(radiansY);        
        }
        else // 2d circle
        {
            x = playerPosition.x + radius * Mathf.Cos(radiansX);
            y = 0;
            z = playerPosition.y + radius * Mathf.Sin(radiansX);
        }

        Vector3 ballPosition = new Vector3(x, y, z);
        
        // use a static projectile for now
        ball.GetComponent<ProjectileBehaviour>().Init(ballPosition, ballPosition, 0, ball, csvWriter);
        ball.GetComponent<ProjectileBehaviour>().SetVisible(true);
    
        ball.AddComponent<AudioSource>();
        if (uniqueSounds)
            ball.GetComponent<AudioSource>().clip = audioClips[id];
        else
            ball.GetComponent<AudioSource>().clip = audioClips[0];
 
        ball.GetComponent<AudioSource>().playOnAwake = true;
        ball.GetComponent<AudioSource>().loop = true;
        ball.AddComponent<OSPAudioSource>();
        
        balls.Add(ball);
        if (!directionIs3D)
            Debug.Log("2D Ball Spawned at " + radiansX / Mathf.PI + "pi radiansX with position " + ballPosition.ToString());
        else
            Debug.Log("3D Ball Spawned at " + radiansX / Mathf.PI + "pi radiansX, "+ radiansY/Mathf.PI +"pi radiansY with position " + ballPosition.ToString());

    }


}