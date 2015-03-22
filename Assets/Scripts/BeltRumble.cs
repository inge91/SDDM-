using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;

// A (very) basic version of a class to control the rumble of our haptic belt


// this class should probably be called from somewhere else, instead of being part of the general update loop
public class BeltRumble : MonoBehaviour 
{

    bool Enabled = false; // is rumble enabled/disabled
    bool Active = false;
    bool IsConstant = true;
    GamePadState PreviousFrontState;
    bool BeltConnected = false;

    // controller variables
    const PlayerIndex FrontIndex = PlayerIndex.One;
    const PlayerIndex LeftIndex  = PlayerIndex.Two;
    const PlayerIndex RightIndex = PlayerIndex.Four;
    const PlayerIndex BackIndex  = PlayerIndex.Three;
    GamePadState FrontState;
    GamePadState LeftState;
    GamePadState RightState;
    GamePadState BackState;

    float LastActivationTime = 0;      // The time at of the last call to set rumble
    float AlternationDuration = 0.1f;  // for how long rumble is triggered in alternating mode
    float AlternationAmplitude = 0.8f; // how often rumble is triggered in alterating mode


	void Start () 
    {
        PreviousFrontState = GamePad.GetState(PlayerIndex.One);
	}
	

	void Update () 
    {
        // Get the current gamepad states.
        FrontState = GamePad.GetState(FrontIndex);
        LeftState = GamePad.GetState(LeftIndex);
        RightState = GamePad.GetState(RightIndex);
        BackState = GamePad.GetState(BackIndex);

        CheckGuideButton();//temporary method to enable disable rumble with the guide button
        BeltConnected = FrontState.IsConnected && LeftState.IsConnected && RightState.IsConnected && BackState.IsConnected;

        if (Enabled)
        {
            if (BeltConnected) // all 4 controller connected
                ThumbStickRumble(IsConstant);
            else if (FrontState.IsConnected) // only one connected
                TriggerRumble(FrontIndex);
        }
        
        PreviousFrontState = FrontState;
	}

#region experimental methods

    //temporary method to enable disable rumble
    void CheckGuideButton()
    {
        if (FrontState.IsConnected)
        {
            if (FrontState.Buttons.Guide == ButtonState.Pressed && PreviousFrontState.Buttons.Guide == ButtonState.Released)
            {
                if (!Enabled)
                {
                    Enabled = true;
                    Debug.Log("Enabled rumble");
                }
                else if (IsConstant)
                {
                    IsConstant = false;
                    Debug.Log("Switch mode to alternating ");
                }
                else
                {
                    Enabled = false;
                    Debug.Log("Disabled rumble");
                }
            }
        }
    }

    // a method to eperiment with rumble using the input of the first controller
    void ThumbStickRumble(bool constant)
    {       
            Vector2 direction = new Vector2(FrontState.ThumbSticks.Left.X, FrontState.ThumbSticks.Left.Y);
            if (constant)
                ConstantRumble(direction);
            else
                AlternatingRumble(direction);
    }

    // control rumble motors of the first controller with the triggers
    void TriggerRumble(PlayerIndex playerIndex)
    {
        GamePad.SetVibration(playerIndex, FrontState.Triggers.Left, FrontState.Triggers.Right);
    }
#endregion


    // rumble is simply determined by the input
    void ConstantRumble(Vector2 direction)
    {
        SetBeltRumble(direction);
    }

    // like ConstantRumble, but rumble turns on or off with a certain amplitude
    void AlternatingRumble(Vector2 direction)
    {
        // switch whether rumble is active right now
        if (Time.time - LastActivationTime > AlternationAmplitude)
            Active = !Active;
        // only rumble for a certain duration when active
        if (Active && Time.time - LastActivationTime > AlternationDuration)
            SetBeltRumble(direction);
        else
            StopAllVibrations();
    }


    #region coroutines to replace current methods?

    // make a constant rumble for a certain duration
    IEnumerator ConstantRumbleRoutine(Vector2 direction, float duration)
    {
        float startTime = Time.time;
        SetBeltRumble(direction);
        while (Time.time - startTime <= duration)
        {
            yield return null;
        }
        StopAllVibrations();
    }


    // method for alternating rumbling for a certain duration
    IEnumerator AlternatingRumbleRoutine(Vector2 direction, float routineDuration, float rumbleDuration, float amplitude)
    {
        float startTime = Time.time;
        float lastTime = Time.time;
        while (Time.time - startTime <= routineDuration)
        {
            if (Time.time - lastTime >= amplitude)
            {
                Active = !Active;
                if (Active)
                    SetBeltRumble(direction);
                else
                    StopAllVibrations();

                lastTime = Time.time;
            }
            yield return null;
        }
        StopAllVibrations();
    }

    #endregion


    // set rumble of 4 controllers making up the belt using a direction on a unit sphere
    private void SetBeltRumble(Vector2 direction)
    {
        LastActivationTime = Time.time;
        if(direction.magnitude > 1)
            direction.Normalize(); // vector on or within unit circle around center
      
        //set rumble intensities as distance in range [0,1]
        float rumbleIntensityX = Math.Abs(direction.x);
        float rumbleIntensityY = Math.Abs(direction.y);

        // y coordinate determines front and back
        if (direction.y > 0)
            GamePad.SetVibration(FrontIndex, rumbleIntensityY, rumbleIntensityY);
        else
            StopVibration(FrontIndex);
        if (direction.y < 0)
            GamePad.SetVibration(BackIndex, rumbleIntensityY, rumbleIntensityY);
        else
            StopVibration(BackIndex);

        //x coordinate determines left and right
        if (direction.x < 0)
            GamePad.SetVibration(LeftIndex, rumbleIntensityX, rumbleIntensityX);
        else
            StopVibration(LeftIndex);
        if (direction.x > 0)
            GamePad.SetVibration(RightIndex, rumbleIntensityX, rumbleIntensityX);
        else
            StopVibration(RightIndex);
    }



    // disable vibration on a single controller
    void StopVibration(PlayerIndex playerIndex)
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    // disable virbation on all 4 controllers
    void StopAllVibrations()
    {
        Active = false;
        StopVibration(FrontIndex);
        StopVibration(LeftIndex);
        StopVibration(RightIndex);
        StopVibration(BackIndex);
    }

}
