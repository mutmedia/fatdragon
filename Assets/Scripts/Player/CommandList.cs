using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Assets.Scripts.Enums;

public class CommandList 
{
    int CommandIndex;
    public ArrayList List;
    public List<Player> Players = new List<Player>();

    private System.Random _random = new System.Random();
    private Array _valuesCommandTypes = Enum.GetValues(typeof(CommandType));

    public CommandList()
    {
        List = new ArrayList();
        CommandIndex = -1;
        Command newCommand = getRandomCommand();
        this.Add(newCommand);
        Debug.Log(newCommand.Left+" "+newCommand.Right);
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
        foreach(Player player in Players)
        {
            bool result = false;
            Command command = (Command)a.Command;
            Command item = (Command)List[CommandIndex];
            if (command.Left == item.Left && command.Right == item.Right)
            {
                result = true;
            }

            player.ResolveCommandResult(result);
        }

    }

    public void Add(Command command)
    {
        CommandIndex++;
        List.Add(command);
    }

    public void Add(Player player)
    {
        Players.Add(player);
        player.CommandEventHandler += Compare;
    }

    public void Next()
    {
        CommandIndex++;

        if (CommandIndex >= List.Count)
        {
            CommandIndex = 0;
        }
    }
}
