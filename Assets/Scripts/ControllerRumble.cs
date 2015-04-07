using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;


public class ControllerRumble : MonoBehaviour 
{
    public bool Enabled;
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
	}


    public void PlayerShoot()
    {
         StartCoroutine(TriggerShoot());
    }

    //example of shooting rumble
    private IEnumerator TriggerShoot()
    {
        if (Enabled)
        {
            routineActive = true;
            SetRumble(0.3f, 1.0f);
            yield return new WaitForSeconds(0.2f);
            //SetRumble(0.5f, 0.5f);
            //yield return new WaitForSeconds(0.1f);
            StopRumble();
            yield return new WaitForSeconds(0.8f);
            routineActive = false;
        }
    }


 




    // make a constant rumble for a certain duration
    private IEnumerator ConstantRumbleRoutine(float leftIntensity, float rightIntensity, float duration)
    {
        if (Enabled)
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
    }


    // method for alternating rumbling for a certain duration
    // routineDuration is the total duration, rumbleDuration how long a single rumble burst lasts, stopInterval the length of the pause between rumbles
    private IEnumerator AlternatingRumbleRoutine(float leftIntensity, float rightIntensity, float routineDuration, float rumbleDuration, float stopInterval)
    {
        if (Enabled)
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
    }


    // set rumble of 4 controllers making up the belt using a direction on a unit sphere
    private void SetRumble(float leftIntensity, float rightIntensity)
    {      
        GamePad.SetVibration(playerIndex, leftIntensity, rightIntensity);
    }


    //prevent rumble from staying active on game quit
    void OnApplicationQuit()
    {
        StopRumble();
    }

    // disable vibration on a single controller
    public void StopRumble()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

}
