using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;

public class CommandEventArgs : EventArgs
{
    public Command Command { get; set; }
}

public class Player : MonoBehaviour, IControllable {

    private bool _commandSuccess;
    public float timeLimitRange;
    public float timePace;
    private float _lastTime;
    private Command _command = new Command();
    private Command _lastCommand = new Command();

    public event EventHandler<CommandEventArgs> CommandEventHandler;
    
    // Use this for initialization
	void Start () {
        _lastTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (_lastCommand != _command)
	    {
	        if (CommandEventHandler != null)
	        {
	            CommandEventHandler.Invoke(this, new CommandEventArgs()
	            {
	                Command = _command,
	            });
	        }
	    }
	    _lastCommand = _command;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        string spriteName = "Idle";
        bool flip = false;
        
        if (_command.Left == CommandType.up && _command.Right == CommandType.down)
        {
            spriteName = "LeftArmUp";
            flip = false;
        } 
        else if(_command.Left == CommandType.up && _command.Right == CommandType.right)
        {
            spriteName = "LeftArmUp";
            flip = true;
        }
        else if (_command.Left == CommandType.left && _command.Right == CommandType.down)
        {
            spriteName = "LeftArmLeft";
            flip = false;
        }
        else if (_command.Left == CommandType.right && _command.Right == CommandType.right)
        {
            spriteName = "LeftArmLeft";
            flip = true;
        }
        else if (_command.Left == CommandType.down && _command.Right == CommandType.down)
        {
            spriteName = "LeftArmDown";
            flip = false;
        }
        else if (_command.Left == CommandType.down && _command.Right == CommandType.right)
        {
            spriteName = "LeftArmDown";
            flip = true;
        }

        Sprite sprite = Resources.Load(spriteName, typeof(Sprite)) as Sprite;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<SpriteRenderer>().flipX = flip;
    }

    public void ResolveCommandResult(bool result)
    {
        float actualTime = Time.time;
        bool didPlayerGetRight = false;
        if(result)
        {
            float timeDelta = actualTime - _lastTime;
            if(timePace - timeLimitRange/2 <= timeDelta && timeDelta <= timePace + timeLimitRange / 2)
            {
                didPlayerGetRight = true;
            }
            _lastTime = actualTime;
        }

        if(didPlayerGetRight)
        {
            Debug.Log("Success!");
        }
        else
        {
            Debug.Log("Fail!");
        }
    }

    public void MoveLeftSide(CommandType command, GameState state)
    {
        _command.Left = command;
    }

    public void MoveRightSide(CommandType command, GameState state)
    {
        _command.Right = command;
    }
}
