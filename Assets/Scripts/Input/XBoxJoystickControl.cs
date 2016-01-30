using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

class XBoxJoystickControl : IControl
{
    private float triggerThreshold = 0.3f;

    private static int indexCounter = 0;

    private IControllable controllable;
    private int index;
    private float deadzone;

    public event EventHandler PauseRequestEvent;

    private XBoxJoystickControl(int index, float deadzone)
    {
        this.index = index;
        this.deadzone = deadzone;
    }

    public static XBoxJoystickControl GetControl()
    {
        var joysticks = Input.GetJoystickNames();
        if ((indexCounter + 1) > joysticks.Length)
            return null;

        return new XBoxJoystickControl(++indexCounter, 0.3f);
    }

    public static void Reset()
    {
        indexCounter = 0;
    }

    public void SetControllable(IControllable controllable)
    {
        this.controllable = controllable;
    }

    public void Update(GameState state)
    {
        if(controllable == null)
        {
            throw new NullReferenceException();
        }

        // Get X and Y for left analog stick
        float x = Input.GetAxisRaw("Joystick" + index + "XAxis");
        float y = Input.GetAxisRaw("Joystick" + index + "YAxis");

        if (y > deadzone)
        {
            controllable.LeftSideUp(state);
        }

        if (y < -deadzone)
        {
            controllable.LeftSideDown(state);
        }

        if (x > deadzone)
        {
            controllable.LeftSideRight(state);
        }

        if (x < -deadzone)
        {
            controllable.LeftSideLeft(state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire0") > triggerThreshold)
        {
            controllable.RightSideUp(state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire1") > triggerThreshold)
        {
            controllable.RightSideDown(state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire2") > triggerThreshold)
        {
            controllable.RightSideLeft(state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire3") > triggerThreshold)
        {
            controllable.RightSideRight(state);
        }

        if (Input.GetButton("Start"))
        {
            if (PauseRequestEvent != null)
            {
                PauseRequestEvent(this, null);
            }
            
        }
    }
}