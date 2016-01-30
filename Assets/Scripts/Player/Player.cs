using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Player;

public class Player : MonoBehaviour, IControllable {

	private Command _command = new Command();
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MoveLeftSide(CommandType command, GameState state)
    {
        _command.Left = command;
    }

    public void MoveRightSide(CommandType command, GameState state)
    {
        _command.Left = command;
    }
}
