using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;

// A (very) basic version of a class to control the rumble of our haptic belt


// this class should probably be called from somewhere else, instead of being part of the general update loop
public class BeltRumble : MonoBehaviour 
{

    bool Enabled = false; // is rumble enabled/disabled
    GamePadState PreviousPlayerState;
    bool BeltConnected = false;

    PlayerIndex FrontIndex = PlayerIndex.One;
    PlayerIndex LeftIndex = PlayerIndex.Two;
    PlayerIndex RightIndex = PlayerIndex.Four;
    PlayerIndex BackIndex = PlayerIndex.Three;

	void Start () 
    {
        PreviousPlayerState = GamePad.GetState(PlayerIndex.One);
	}
	

	void Update () 
    {
        RumbleTest();       
	}

    // a method to eperiment with rumble using the input of the first controller
    void RumbleTest()
    {
        // Get the current gamepad states.
        GamePadState frontState = GamePad.GetState(FrontIndex);
        GamePadState leftState = GamePad.GetState(LeftIndex);
        GamePadState rightState = GamePad.GetState(RightIndex);
        GamePadState backState = GamePad.GetState(BackIndex);
        GamePadState playerState = frontState;

        if (playerState.IsConnected)
        {
            if (playerState.Buttons.Guide == ButtonState.Pressed && PreviousPlayerState.Buttons.Guide == ButtonState.Released)
            {
                Enabled = !Enabled;
                Debug.Log("set rumble enabled to " + Enabled);
            }
            BeltConnected = frontState.IsConnected && leftState.IsConnected && rightState.IsConnected && backState.IsConnected;

            if (Enabled)
            {
                if (BeltConnected) // control the belt witht he left thumbstick if it is connected
                {
                    Vector2 direction = new Vector2(playerState.ThumbSticks.Left.X, playerState.ThumbSticks.Left.Y);
                    SetBeltRumble(direction);
                }
                else // let the triggers control rumble if only one controller is connected
                {
                    GamePad.SetVibration(PlayerIndex.One, playerState.Triggers.Left, playerState.Triggers.Right);
                }
            }
        }

        PreviousPlayerState = playerState;
    }

    // set rumble of 4 controllers making up the belt using a direction on a unit sphere
    void SetBeltRumble(Vector2 direction)
    {
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
        StopVibration(PlayerIndex.One);
        StopVibration(PlayerIndex.Two);
        StopVibration(PlayerIndex.Three);
        StopVibration(PlayerIndex.Four);
    }

}
