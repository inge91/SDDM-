using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;


public class ControllerRumble : MonoBehaviour 
{

    bool routineActive = false;

    // controller variables
    public PlayerIndex playerIndex = PlayerIndex.One;
    public GamePadState state;

	void Start () 
    {
        state = GamePad.GetState(playerIndex);
    }
	

	void Update () 
    {
        // Get the current gamepad states.
        state = GamePad.GetState(playerIndex);

        //live updated trigger rumble
       /* if (!routineActive && state.Triggers.Right > 0.9f)
        {
            StartCoroutine(TriggerShoot());
        }*/
	}


    // example method for when player shoots
    public void PlayerShoot()
    {
        StartCoroutine(TriggerShoot());
    }

    //example of shooting rumble
    private IEnumerator TriggerShoot()
    {
        routineActive = true;
        SetRumble(0.5f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        SetRumble(1.0f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        StopRumble();
        yield return new WaitForSeconds(1);
        routineActive = false;
    }


 




    // make a constant rumble for a certain duration
    private IEnumerator ConstantRumbleRoutine(float leftIntensity, float rightIntensity, float duration)
    {
        routineActive = true;
        float startTime = Time.time;
        SetRumble(leftIntensity, rightIntensity);
        while (Time.time - startTime <= duration)
        {
            yield return null;
        }
        StopRumble();
        routineActive = false;
    }


    // method for alternating rumbling for a certain duration
    // routineDuration is the total duration, rumbleDuration how long a single rumble burst lasts, stopInterval the length of the pause between rumbles
    private IEnumerator AlternatingRumbleRoutine(float leftIntensity, float rightIntensity, float routineDuration, float rumbleDuration, float stopInterval)
    {
        routineActive = true;
        float startTime = Time.time;
        float lastActivation = Time.time;
        bool rumbleActive = true;
        SetRumble(leftIntensity, rightIntensity);
        // keep activating and disactivating rumble until the routine time is over
        while (Time.time - startTime <= routineDuration)
        {
            if (rumbleActive && Time.time - lastActivation >= rumbleDuration)
            {
                StopRumble();
                rumbleActive = false;
            }
            if (!rumbleActive && Time.time - lastActivation >= rumbleDuration + stopInterval)
            {
                rumbleActive = true;
                StopRumble();
                lastActivation = Time.time;
            }
            yield return null;
        }
        StopRumble();
        routineActive = false;
    }


    // set rumble of 4 controllers making up the belt using a direction on a unit sphere
    private void SetRumble(float leftIntensity, float rightIntensity)
    {      
        GamePad.SetVibration(playerIndex, leftIntensity, rightIntensity);
    }
        

    // disable vibration on a single controller
    public void StopRumble()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

}
