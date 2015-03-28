﻿using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;


public class BeltRumble : MonoBehaviour 
{

    // controller variables
    const PlayerIndex FrontIndex = PlayerIndex.One;
    const PlayerIndex LeftIndex  = PlayerIndex.Two;
    const PlayerIndex RightIndex = PlayerIndex.Four;
    const PlayerIndex BackIndex  = PlayerIndex.Three;
    GamePadState FrontState;
    GamePadState LeftState;
    GamePadState RightState;
    GamePadState BackState;



	void Start () 
    {}
	

	void Update () 
    {
        // Get the current gamepad states.
        FrontState = GamePad.GetState(FrontIndex);
        LeftState = GamePad.GetState(LeftIndex);
        RightState = GamePad.GetState(RightIndex);
        BackState = GamePad.GetState(BackIndex);

	}

    // example method for when player gets hit
    public void PlayerHit(Vector3 direction, float intensity = 0.8f)
    {
        Vector2 planeDirection = new Vector2(direction.x, direction.z);
       StartCoroutine(ConstantRumbleRoutine(planeDirection, intensity, 0.2f));
    }

    // make a constant rumble for a certain duration
    private IEnumerator ConstantRumbleRoutine(Vector2 direction, float intensity, float duration)
    {
        float startTime = Time.time;
        SetBeltRumble(direction, intensity);
        while (Time.time - startTime <= duration)
        {
            yield return null;
        }
        StopAllRumble();
    }


    // method for alternating rumbling for a certain duration
    // routineDuration is the total duration, rumbleDuration how long a single rumble burst lasts, stopInterval the length of the pause between rumbles
    private IEnumerator AlternatingRumbleRoutine(Vector2 direction, float intensity, float routineDuration, float rumbleDuration, float stopInterval)
    {
        float startTime = Time.time;
        float lastActivation = Time.time;
        bool rumbleActive = true;
        SetBeltRumble(direction, intensity);
        // keep activating and disactivating rumble until the routine time is over
        while (Time.time - startTime <= routineDuration)
        {
            if (rumbleActive && Time.time - lastActivation >= rumbleDuration)
            {
                StopAllRumble();
                rumbleActive = false;
            }
            if (!rumbleActive && Time.time - lastActivation >= rumbleDuration + stopInterval)
            {
                rumbleActive = true;
                SetBeltRumble(direction, intensity);
                lastActivation = Time.time;
            }
            yield return null;
        }
        StopAllRumble();
    }


    // set rumble of 4 controllers making up the belt using a direction on a unit sphere
    private void SetBeltRumble(Vector2 direction, float intensity)
    {
        // clamp intensity to range [0,1]
        intensity = intensity < 0 ? 0 : (intensity > 1 ? 1 : intensity);

        if (OneConnected()) //backup if only one controller is connected
        {
            GamePad.SetVibration(FrontIndex, intensity, intensity);
        }
        else // rumble the belt
        {
            float rumbleIntensityX, rumbleIntensityY;

            // set direction as vector on unit circle      
            if (direction.magnitude > 0)
            {
                direction.Normalize();
                //base intensity on direction
                rumbleIntensityX = Math.Abs(direction.x) * intensity;
                rumbleIntensityY = Math.Abs(direction.y) * intensity;

                // y coordinate determines front and back
                if (direction.y > 0)
                    GamePad.SetVibration(FrontIndex, rumbleIntensityY, rumbleIntensityY);
                else
                    StopRumble(FrontIndex);
                if (direction.y < 0)
                    GamePad.SetVibration(BackIndex, rumbleIntensityY, rumbleIntensityY);
                else
                    StopRumble(BackIndex);

                //x coordinate determines left and right
                if (direction.x < 0)
                    GamePad.SetVibration(LeftIndex, rumbleIntensityX, rumbleIntensityX);
                else
                    StopRumble(LeftIndex);
                if (direction.x > 0)
                    GamePad.SetVibration(RightIndex, rumbleIntensityX, rumbleIntensityX);
                else
                    StopRumble(RightIndex);

            }
            else
            {
                GamePad.SetVibration(FrontIndex, intensity, intensity);
                GamePad.SetVibration(RightIndex, intensity, intensity);
                GamePad.SetVibration(LeftIndex, intensity, intensity);
                GamePad.SetVibration(BackIndex, intensity, intensity);
            }
        }
    }

    //check if all 4 controllers for the belt are connected
    private bool BeltConnected()
    {
        return FrontState.IsConnected && LeftState.IsConnected && RightState.IsConnected && BackState.IsConnected;
    }

    //check if only the first controller is connected
    private bool OneConnected()
    {
        return FrontState.IsConnected && !LeftState.IsConnected && !RightState.IsConnected && !BackState.IsConnected;
    }

    // disable vibration on a single controller
    private void StopRumble(PlayerIndex playerIndex = FrontIndex)
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    // disable virbation on all 4 controllers
    private void StopAllRumble()
    {
        StopRumble(FrontIndex);
        StopRumble(LeftIndex);
        StopRumble(RightIndex);
        StopRumble(BackIndex);
    }

}
