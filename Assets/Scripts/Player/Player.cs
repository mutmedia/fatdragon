using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Player;

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
	    if (_lastCommand == _command) return;
        if (CommandEventHandler != null)
        {
            CommandEventHandler.Invoke(new CommandEventArgs()
            {
                Command = _command;
            });
        }
	    _lastCommand = _command;
	}

    public void MoveLeftSide(CommandType command, GameState state)
    {
        if(command != CommandType.none && _command.Left == CommandType.none)
            _command.Left = command;
    }

    public void MoveRightSide(CommandType command, GameState state)
    {
        _command.Left = command;
    }
}
