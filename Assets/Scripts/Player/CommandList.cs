using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts;

public class CommandList : MonoBehaviour
{
    int CommandIndex;
    public ArrayList List;
    public List<Player> Players = new List<Player>();

    public TimeManager timeManager;

    private System.Random _random = new System.Random();
    private Array _valuesCommandTypes = Enum.GetValues(typeof(CommandType));

    public EventHandler<OnListOverEventArgs> OnListOverEventHandler;
    public EventHandler<ResolveCommandEventArgs> ResolveCommandEventHandler;

    private bool _isCurrentRunSuccessful;
    private bool _updateCommandDemand;
    private bool _currentResult;

    void Start()
    {
        List = new ArrayList();
        CommandIndex = -1;
        Command newCommand = getRandomCommand();
        this.Add(newCommand);
        _updateCommandDemand = true;
        Debug.Log(newCommand.Left + " " + newCommand.Right);
    }

    void Update()
    {
        if(_updateCommandDemand)
        {
            int i = 0;
            foreach(Transform child in transform)
            {
                Command item = (Command)List[i++];
                UpdateCommandSprite(child, item);
            }
            _updateCommandDemand = false;
        }
        if(timeManager.flag)
        {
            
        }
    }

    void UpdateCommandSprite(Transform child, Command item)
    {
        float angleLeft = 0;
        float angleRight = 0;
        switch (item.Left)
        {
            case CommandType.down:
                angleLeft = 180;
                break;
            case CommandType.left:
                angleLeft = 90;
                break;
            case CommandType.right:
                angleLeft = 270;
                break;
        }
        switch (item.Right)
        {
            case CommandType.down:
                angleRight = 180;
                break;
            case CommandType.left:
                angleRight = 90;
                break;
            case CommandType.right:
                angleRight = 270;
                break;
        }
        child.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));
        child.GetChild(1).Rotate(new Vector3(0, 0, angleRight));
    }

    public Command getRandomCommand()
    {
        int Length = _valuesCommandTypes.Length - 1;
        CommandType randomRight = (CommandType)_valuesCommandTypes.GetValue(_random.Next(Length));
        CommandType randomLeft = (CommandType)_valuesCommandTypes.GetValue(_random.Next(Length));
        return new Command(randomLeft, randomRight);
    }

    public void Compare(object sender, CommandEventArgs a)
    {
        var player = (Player) sender;

        bool result = false;
        Command command = (Command)a.Command;
        Command item = (Command)List[CommandIndex];
        if (command.Left == item.Left && command.Right == item.Right)
        {
            result = true;
        }

        if (result && !timeManager.flag)
        {
            result = false;
        }

        _currentResult = result;

        if (ResolveCommandEventHandler != null)
        {
            ResolveCommandEventHandler.Invoke(this, new ResolveCommandEventArgs()
            {
                IsCorrect = result,
            });
        }
    }

    public void OnTimerChangeEvent(object sender, EventArgs e)
    {
        _isCurrentRunSuccessful = _currentResult;
        Next();
    }

    public void Add(Command command)
    {
        CommandIndex++;
        List.Add(command);
    }

    public void Next()
    {
        CommandIndex++;

        if (CommandIndex >= List.Count)
        {
            CommandIndex = 0;
            if (OnListOverEventHandler != null)
            {
                OnListOverEventHandler.Invoke(this, new OnListOverEventArgs()
                {
                    IsSuccessful = _isCurrentRunSuccessful,
                } );
            }
            _isCurrentRunSuccessful = true;
        }
    }

    public void Add(Player player)
    {
        Players.Add(player);
        player.CommandEventHandler += Compare;
    }

}

public class ResolveCommandEventArgs : EventArgs
{
    public bool IsCorrect { get; set; }
    public Player ResolvingPlayer { get; set; }
}


public class OnListOverEventArgs : EventArgs
{
    public bool IsSuccessful { get; set; }
}
