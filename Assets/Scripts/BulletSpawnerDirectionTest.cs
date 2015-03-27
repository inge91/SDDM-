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
    public float startDelay;        // wait a little before starting the spawning etc

   
    public int amountOfSounds;      //amount of sound emitting objects spawned
    public bool uniqueSounds;       // do they play unique sounds
    public float audioSpawnDelay;  //delay inbetween spawns
    public List<AudioClip> audioClips;


    

    // private variables in code?
    private List<GameObject> balls;
    private Vector3 playerPosition;
    private CsvWriter csvWriter;



    void Start()
    {
        playerPosition = playerObject.transform.position;
        /*csvWriter = new CsvWriter("AudioDirectionEstimateTest.txt", "Object number, Actual angle, EstimateAngle, Error");*/
        balls = new List<GameObject>();

        StartCoroutine(spawnBallsAfterInterval());
    }

    IEnumerator spawnBallsAfterInterval()
    {
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
        yield return null;
    }


    void Update()
    {
        //some aiming thingy

    }

    private void spawnBall(int id)
    {
        GameObject ball = Instantiate(objectToSpawn);

        float x, z, y = 0;

        // start position should be on a circle around the user.  
        float radians = Random.Range(0, Mathf.PI * 2);
        x = playerPosition.x + radius * Mathf.Cos(radians);
        z = playerPosition.y + radius * Mathf.Sin(radians);

        Vector3 ballPosition = new Vector3(x, y, z);
        // use a static projectile for now
        ball.GetComponent<ProjectileBehaviour>().Init(ballPosition, ballPosition, 0, ball, csvWriter);
        ball.GetComponent<ProjectileBehaviour>().SetVisible(false);
    

        ball.AddComponent<AudioSource>();
        if (uniqueSounds)
            ball.GetComponent<AudioSource>().clip = audioClips[id];
        else
            ball.GetComponent<AudioSource>().clip = audioClips[0]; 
        ball.GetComponent<AudioSource>().playOnAwake = true;
        ball.GetComponent<AudioSource>().loop = true;
        ball.AddComponent<OSPAudioSource>();
        balls.Add(ball);
        Debug.Log("Ball Spanwned at " + radians + " radians with vector " + ballPosition.ToString());
    }


}