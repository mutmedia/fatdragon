using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts;

public class CommandEventArgs : EventArgs
{
    public Command Command { get; set; }
}

public class Player : MonoBehaviour, IControllable {

    private bool _commandSuccess;

    private float _lastTime;
    private Command _command = new Command();
    private Command _lastCommand = new Command();

    public event EventHandler<CommandEventArgs> CommandEventHandler;
    
    // Use this for initialization
	void Start () {
        _lastTime = Time.time;
    }

    private bool resolveDeathSprite = false;
    private float timeOfDeath;

    private bool inputChanged;
	// Update is called once per frame
	void Update ()
	{
        if (inputChanged)
        {
            inputChanged = false;
            if (_command.Right != CommandType.none && _command.Left != CommandType.none)
            {
                if (CommandEventHandler != null)
                {
                    CommandEventHandler.Invoke(this, new CommandEventArgs()
                    {
                        Command = _command,
                    });
                }
            }
        }
        
	    _lastCommand = _command;
        if(!resolveDeathSprite)
        {
            UpdateSprite();
        }
        else
        {
            float presentTime = Time.time;
            float timeDelta = presentTime - timeOfDeath;
            if (timeDelta - 0.9f > -0.05f && timeDelta - 0.9f < 0.05f)
            {
                string spriteName = "Blackened";
                Sprite sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
                GetComponent<SpriteRenderer>().sprite = sprite;
                resolveDeathSprite = false;
            }
               
        }
        
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
        else if (_command.Left == CommandType.right && _command.Right == CommandType.down)
        {
            spriteName = "LeftArmLeft";
            flip = true;
        }
        else if (_command.Left == CommandType.left && _command.Right == CommandType.right)
        {
            spriteName = "LeftArmLeft";
        }

        if(spriteName.CompareTo("Idle") != 0)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
            GetComponent<SpriteRenderer>().sprite = sprite;
            GetComponent<SpriteRenderer>().color = this.Number == 1 ? Color.blue : Color.red;
            GetComponent<SpriteRenderer>().transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
        }


    }

    public void ResolveCommandResult(bool result)
    {
        
        bool didPlayerGetRight = result;

        if(didPlayerGetRight)
        {
            //Debug.Log("Success!");
        }
        else
        {
            //Debug.Log("Fail!");
        }
    }

    public void OnDeathResolve(object sender, EndGameEventArgs e)
    {
        resolveDeathSprite = true;
        timeOfDeath = e.timeOfDeath;
    }

    public void OnTimeSucessEvent(object sender, EventArgs e)
    {

    }

    public void OnTimeFailEvent(object sender, EventArgs e)
    {

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

    internal static Player Create(Vector3 initialPos, int index)
    {
        var playerPrefab = Resources.Load<Player>("Prefabs/Player");

        var player = Instantiate<Player>(playerPrefab);
        player.transform.position = initialPos;
        player.Number = index;


        return player;
    }

    public int Number { get; set; }
}
