using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;

public class Player : MonoBehaviour, IControllable {
    public class CommandEventArgs : EventArgs
    {
        public Command Command { get; set; }
    }

    private Command _command = new Command();
    private Command _lastCommand = new Command();

    public event EventHandler<CommandEventArgs> CommandEventHandler;
    
    // Use this for initialization
	void Start () {
	
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
	}

    public void MoveLeftSide(CommandType command, GameState state)
    {
        _command.Left = command;
    }

    public void MoveRightSide(CommandType command, GameState state)
    {
        _command.Left = command;
    }

    internal static Player Create(Vector3 initialPos)
    {
        var playerPrefab = Resources.Load<Player>("Prefabs/Player");

        var player = Instantiate<Player>(playerPrefab);
        player.transform.position = initialPos;


        return player;
    }
}
