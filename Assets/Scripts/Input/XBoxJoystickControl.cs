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
            controllable.MoveLeftSide(CommandType.down, state);
        }
        else if (y < -deadzone)
        {
            controllable.MoveLeftSide(CommandType.up, state);
        }
        else if (x < -deadzone)
        {
            controllable.MoveLeftSide(CommandType.left, state);
        }
        else if (x > deadzone)
        {
            controllable.MoveLeftSide(CommandType.right, state);
        }
        else
        {
            controllable.MoveLeftSide(CommandType.none, state);
        }

        if (Input.GetButton("Joystick" + index + "Button1"))
        {
            controllable.MoveRightSide(CommandType.right, state);
        }
        else if (Input.GetButton("Joystick" + index + "Button2"))
        {
            controllable.MoveRightSide(CommandType.left, state);
        }
        else if (Input.GetButton("Joystick" + index + "Button3"))
        {
            controllable.MoveRightSide(CommandType.up, state);
        }
        else if (Input.GetButton("Joystick" + index + "Button0"))
        {
            controllable.MoveRightSide(CommandType.down, state);
        }
        else
        {
            controllable.MoveRightSide(CommandType.none, state);
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