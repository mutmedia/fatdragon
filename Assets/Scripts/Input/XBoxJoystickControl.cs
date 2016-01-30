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
            controllable.MoveLeftSide(CommandType.up, state);
        }

        if (y < -deadzone)
        {
            controllable.MoveLeftSide(CommandType.down, state);
        }

        if (x > deadzone)
        {
            controllable.MoveLeftSide(CommandType.left, state);
        }

        if (x < -deadzone)
        {
            controllable.MoveLeftSide(CommandType.right, state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire0") > triggerThreshold)
        {
            controllable.MoveRightSide(CommandType.right, state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire1") > triggerThreshold)
        {
            controllable.MoveRightSide(CommandType.left, state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire2") > triggerThreshold)
        {
            controllable.MoveRightSide(CommandType.up, state);
        }

        if (Input.GetAxisRaw("Joystick" + index + "Fire3") > triggerThreshold)
        {
            controllable.MoveRightSide(CommandType.down, state);
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