using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using UnityEngine;

class KeyboardControl : IControl
{
    private float triggerThreshold = 0.3f;

    private static int indexCounter = 0;

    private IControllable controllable;
    private int index;

    public event EventHandler PauseRequestEvent;

    static bool wasInstantiated = false;
    public static IControl GetControl()
    {
        var joysticks = Input.GetJoystickNames();
        if (joysticks.Length == 0 && !wasInstantiated)
        {
            wasInstantiated = true;
            return new KeyboardControl();
        }

        return null;
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
        float x = Input.GetAxisRaw("Keyboard" + "XAxis");
        float y = Input.GetAxisRaw("Keyboard" + "YAxis");

        if (y > 0)
        {
            controllable.MoveLeftSide(CommandType.down, state);
        }
        else if (y < 0)
        {
            controllable.MoveLeftSide(CommandType.up, state);
        }
        else if (x < 0)
        {
            controllable.MoveLeftSide(CommandType.left, state);
        }
        else if (x > 0)
        {
            controllable.MoveLeftSide(CommandType.right, state);
        }
        else
        {
            controllable.MoveLeftSide(CommandType.none, state);
        }

        if (Input.GetButton("Keyboard" + "Button1"))
        {
            controllable.MoveRightSide(CommandType.right, state);
        }
        else if (Input.GetButton("Keyboard" + "Button2"))
        {
            controllable.MoveRightSide(CommandType.left, state);
        }
        else if (Input.GetButton("Keyboard" + "Button3"))
        {
            controllable.MoveRightSide(CommandType.up, state);
        }
        else if (Input.GetButton("Keyboard" + "Button0"))
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