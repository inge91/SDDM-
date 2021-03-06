﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System.IO;


public class BulletSpawnerDirectionTest : MonoBehaviour
{

    // public variables set in unity scene inspector
   // public bool isExample;
    public bool writeOutput;
    public GameObject objectToSpawn;  //used to generate new objects making sounds
    public GameObject playerObject;
	public GameObject centerEye;
    public GameObject gui;

    public float startDelay;        // wait a little before starting the spawning etc
    public int amountOfObjects;      //amount of sound emitting objects spawned
    public bool uniqueSounds;       // do they play unique sounds
    public float audioSpawnDelay;  //delay inbetween spawns
    public AudioClip selectSound;
    public List<AudioClip> audioClips;

    public float radius;            // constant value of how far away objects are
    public bool directionOnlyFront; // don't place sounds behind the player
    public bool directionIs3D;      //place sound on 2d circle or 3d sphere
    public float heightLimit = 0.125f; // don't use the upper and lower 1/8

    public int iterations;           // how many times a new set of directions will be generated
    // list of positions to use in order of spawning, in pi*radians
    public List<Vector2> positionsInPiRadians; //radX 0 -> right, radx pi -> left
                                               //radY 0 -> top, radY pi -> bottom

    // private variables in code
    private List<GameObject> balls;
    private Vector3 playerPosition;
    private List<GameObject> ballsGuessed;
    private CsvWriter csvWriter;
    private bool spawning = false;
    private int currentIteration = 0;
    private int amountSpawned = 0;
    private float timer = 0;

    void Start()
    {
        playerPosition = playerObject.transform.position;
        ballsGuessed = new List<GameObject>();
        balls = new List<GameObject>();

        if (writeOutput)
        {
            csvWriter = new CsvWriter("Audio" + amountOfObjects + "DirectionEstimateTest", "Iteration number; Actual position; Estimated postion; Angle; Horizontal angle; Vertical angle; Time");
            csvWriter.writeLineToFile("-- " + "amount:" + amountOfObjects + " ,3D:" + directionIs3D + " ,only front:" + directionOnlyFront + " ,unique sounds:" + uniqueSounds);
        }

        if(currentIteration < iterations)
            StartCoroutine(spawnBallsAfterInterval());
    }

    void Update()
    {
        timer += Time.deltaTime;
        // confirm direction with left mouse or A on controller
        if (Input.GetButtonDown("Fire1") && !spawning) 
        {
            Vector3 lookDirection = getLookDirection();
            //Debug.Log("Timer: " + timer);
            markEstimatedBall(lookDirection);
        }
        // all guesses have been made
        if (!spawning && ballsGuessed.Count == balls.Count && balls.Count > 0)
        {
            /*if (writeOutput)
                csvWriter.writeLineToFile("--------Iteration Done------------------");*/
            Debug.Log("completed with iteration");

            DestroyBalls();

            //either spawn a new set, or go back to gui
            if (currentIteration < iterations)
                StartCoroutine(spawnBallsAfterInterval());
            else
            {
                if (writeOutput)
                {
                    csvWriter.writeLineToFile("--------Experiment Done------------------");
                    csvWriter.Close();
                }
                StartCoroutine(enableGui());
            }
        }
    }

    // spawn a given number of balls
    IEnumerator spawnBallsAfterInterval()
    {
        timer = 0;
        currentIteration++;
        spawning = true;
        int amount = amountOfObjects;
        if (uniqueSounds) // if unique sounds are required, you can't spawn more than there are sounds
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

    // spawn a static ball that emits sound
    private void spawnBall(int id)
    {
        Vector3 ballPosition;
        if (positionsInPiRadians.Count > amountSpawned)
        {
            Vector2 radiansxy = positionsInPiRadians[amountSpawned];
            if (directionIs3D)
                ballPosition = positionOnSphere(radiansxy.x * Mathf.PI, radiansxy.y * Mathf.PI);
            else
                ballPosition = positionOnCircle(radiansxy.x * Mathf.PI);
        }
        else
        {
            Debug.Log("No more predetermined positions, used random");
            ballPosition = randomPosition();
        }
       

        amountSpawned++;
        GameObject ball = Instantiate(objectToSpawn);

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
            Debug.Log("2D Ball Spawned at position " + ballPosition.ToString());
        else
            Debug.Log("3D Ball Spawned at position " + ballPosition.ToString());
    }


    //get the direction the oculus is facing
    private Vector3 getLookDirection()
    {
        //get oculus look direction
      //  var centerEyeAnchor = playerObject.transform.GetChild(1);
		Vector3 lookForward = centerEye.transform.forward;//centerEyeAnchor.forward;
        //Vector3 lookForward = Camera.main.transform.forward;
        if (!directionIs3D)
            lookForward.y = 0;

        return lookForward;
    }

    //mark the ball closest to the current looking direction, to determine the most likely error
    private void markEstimatedBall(Vector3 lookDirection)
    {
        if (ballsGuessed.Count < balls.Count) // check if there are any balls left
        {
            // loop to find the ball closest to the looking direction
            GameObject closestBall = null;
            float closestAngle = float.MaxValue;
            for (int i = 0; i < balls.Count; i++)
            {
                GameObject currentBall = balls[i];
                if (!ballsGuessed.Contains(currentBall))
                {
                    float angle = Vector3.Angle(lookDirection, currentBall.transform.position);
                    if (angle < closestAngle)
                    {
                        closestBall = currentBall;
                        closestAngle = angle;
                    }
                }
            }
            // mark the closest ball as found
            if (closestBall != null)
            {
                
                ballsGuessed.Add(closestBall);
                // TODO: Add confirmation sound in inspector
                AudioSource.PlayClipAtPoint(selectSound, playerObject.transform.position);
                if (writeOutput)
                {
                    Vector3 estimatedPosition = lookDirection * radius;
                    
                    // calculate separate angles in the horizontal and vertical plane
          
                    //rotation around y axis (head left to right)
                    double rad2Deg = (180.0 / System.Math.PI);
                    float lookAngle = (float)(System.Math.Atan2(lookDirection.x, lookDirection.z) * rad2Deg);
                    float ballAngle = (float)(System.Math.Atan2(closestBall.transform.position.x, closestBall.transform.position.z) * rad2Deg);
                    float diff = Mathf.DeltaAngle(lookAngle, ballAngle);
                    //rotate to be the same angle around y
                    Vector3 rotatedLook = Quaternion.Euler(0, diff, 0) * lookDirection;
                    float yaw = diff;
                    // rotation around x axis (head up down)
                    float pitch = Vector3.Angle(rotatedLook, closestBall.transform.position);

                    csvWriter.writeLineToFile(currentIteration + "; " + closestBall.transform.position.ToString() + "; " + estimatedPosition.ToString() + "; " +
                        closestAngle +"; " +  yaw + "; " + pitch + "; " + timer);
                }
                Debug.Log("Guess processed " + (balls.Count - ballsGuessed.Count) + " more to go");
            }
        }
    }



    IEnumerator enableGui()
    {
        yield return new WaitForSeconds(1);
        gui.SetActive(true);
        yield return null;
    }

    // remove all sound sources
    void DestroyBalls()
    {
        ballsGuessed.Clear();
        foreach (GameObject ball in balls)
            Destroy(ball);
        balls.Clear();
    }

    //generate a random position around the player on either a 2d circle or 3d sphere
    private Vector3 randomPosition()
    {
        // start position should be on a circle/sphere around the user.  
        float radiansX;
        Vector3 position;

        if (directionOnlyFront)
            radiansX = Random.Range(0, Mathf.PI);
        else
            radiansX = Random.Range(0, Mathf.PI * 2);
        if (directionIs3D)
        {
            float radiansY = Random.Range(heightLimit * Mathf.PI, (1.0f - heightLimit) * Mathf.PI);
            position = positionOnSphere(radiansX, radiansY);
        }
        else // 2d circle
            position = positionOnCircle(radiansX);

        return position;
    }

    // generate a position on a sphere
    private Vector3 positionOnSphere(float radiansX, float radiansY)
    {
        //radX 0 -> right, radx pi -> left
        //radY 0 -> top, radY pi -> bottom
        float x, z, y;
        // radiansY has heightLimit to prevent placement exactly below/above the player
        x = playerPosition.x + radius * Mathf.Cos(radiansX) * Mathf.Sin(radiansY);
        y = playerPosition.z + radius * Mathf.Cos(radiansY);
        z = playerPosition.y + radius * Mathf.Sin(radiansX) * Mathf.Sin(radiansY);
        return new Vector3(x, y, z);
    }

    // generate a position on a circle at y=0
    private Vector3 positionOnCircle(float radians)
    {
        //rad 0 -> right, rad pi -> left
        float x, z, y;
        x = playerPosition.x + radius * Mathf.Cos(radians);
        y = 0;
        z = playerPosition.y + radius * Mathf.Sin(radians);
        return new Vector3(x, y, z);
    }

}