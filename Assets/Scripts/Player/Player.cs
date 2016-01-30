﻿using System;
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


    private bool inputChanged;
	// Update is called once per frame
	void Update ()
	{
        if (inputChanged)
        {
            inputChanged = false;
            if (CommandEventHandler != null)
	        {
	            CommandEventHandler.Invoke(this, new CommandEventArgs()
	            {
	                Command = _command,
	            });
	        }
	    }
        
	    _lastCommand = _command;
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
        if (_lastCommand.Left == command) return;
        _command.Left = command;
        inputChanged = true;
    }

    public void MoveRightSide(CommandType command, GameState state)
    {
        if (_lastCommand.Right == command) return;
        _command.Right = command;
        inputChanged = true;
    }

    internal static Player Create(Vector3 initialPos)
    {
        var playerPrefab = Resources.Load<Player>("Prefabs/Player");

        var player = Instantiate<Player>(playerPrefab);
        player.transform.position = initialPos;


        return player;
    }
}
