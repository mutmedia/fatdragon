using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts;
using UnityEngine.UI;

public class CommandList : MonoBehaviour
{
    int CommandIndex;
    public List<Command> List;
    public List<Player> Players = new List<Player>();

    public TimeManager timeManager;

    private System.Random _random = new System.Random();
    private Array _valuesCommandTypes = Enum.GetValues(typeof(CommandType));

    public EventHandler<OnListOverEventArgs> OnListOverEventHandler;
    public EventHandler<ResolveCommandEventArgs> ResolveCommandEventHandler;

    private bool _isCurrentRunSuccessful;
    private bool _updateCommandDemand;
    private bool _currentResult;

    private GameObject _commandPrefab;

    public Vector3 center;
    public float buttonOffset = 1;

    private Sprite _buttonASprite;
    private Sprite _buttonBSprite;
    private Sprite _arrowSprite;


    public void UpdateSprites()
    {
        var groups = new List<int>();
        var currentGroup = -1;
        var lastCommandFound = CommandType.empty;
        for (int i = 0; i < 8; i++)
        {
            var commandObjectSprite = transform.GetChild(i).GetChild(1);
            
            if (lastCommandFound == List[i].Right || List[i].Right == CommandType.none)
            {
                //Debug.Log(i + ": old group =( -- " + List[i].Right);
                groups[currentGroup]++;
                groups.Add(0);
                commandObjectSprite.GetComponent<SpriteRenderer>().sprite = lastCommandFound == CommandType.right ? _buttonBSprite : _buttonASprite;

            }
            else
            {
                //Debug.Log(i + ": new group =) -- " + List[i].Right);
                groups.Add(1);
                currentGroup = groups.Count - 1;
            }
            lastCommandFound = List[i].Right == CommandType.none ? lastCommandFound : List[i].Right;
        }

        //var logM = "";
        //groups.ForEach(g => logM += g.ToString() + "-");
        //Debug.Log(logM);

        for (int i = 0; i < 8; i++)
        {
            var commandObjectSprite = transform.GetChild(i).GetChild(1);

            //if (groups[i] == 0)
            //{
                //commandObjectSprite.GetComponent<SpriteRenderer>().enabled = false;
                //commandObjectSprite.localPosition = new Vector2(-0.5f, commandObjectSprite.localPosition.y);
                //commandObjectSprite.localScale = new Vector2(1.1f, 1.0f);
            //}
            //else
            //{
                //commandObjectSprite.GetComponent<SpriteRenderer>().enabled = true;
                //commandObjectSprite.localPosition = new Vector2((float)groups[i]/2, commandObjectSprite.localPosition.y);
                //commandObjectSprite.localScale = new Vector2((float)groups[i] * 1.2f, 1.0f);
            //}
        }
    }
    
    public void GetNewCommandObject(Command command)
    {
        var commandObject = (GameObject) Instantiate(_commandPrefab, center, Quaternion.identity);
        commandObject.name = (List.Count - 1).ToString();
        //Debug.Log("new command instantiated");
        commandObject.transform.parent = this.transform;
        commandObject.transform.position = center + new Vector3(buttonOffset, 0, 0) * (List.Count - 1);

        float angleLeft = 0;
        float angleRight = 0;
        switch (command.Left)
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

        Sprite buttonSprite = new Sprite();
        switch (command.Right)
        {
            case CommandType.down:
                buttonSprite = _buttonASprite;
                break;
            case CommandType.right:
                buttonSprite = _buttonBSprite;
                break;
        }


        if (command.Left != CommandType.none)
        {
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            if (command.IsSpecial)
            {
                commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
            }
            else
            {
                commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;
            commandObject.transform.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));
        }
        else
        {
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;
            commandObject.transform.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));
        }

        //commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;
        //commandObject.transform.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));

        if (command.Right != CommandType.none)
        {
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonSprite;
        }
        else
        {
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonSprite;
        }
        
    }

    public void ReplaceCommandObject(Command newCommand, int index)
    {
        List[index] = newCommand;

        var s = "";

        foreach (Command c in List)
        {
            s += "(" + c.Left + ", " + c.Right + ") ";
        }
        Debug.Log(s);
        var commandObject = transform.GetChild(index);

        float angleLeft = 0;
        float angleRight = 0;
        switch (newCommand.Left)
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

        Sprite buttonSprite = new Sprite();
        switch (newCommand.Right)
        {
            case CommandType.down:
                buttonSprite = _buttonASprite;
                break;
            case CommandType.right:
                buttonSprite = _buttonBSprite;
                break;
        }

        if (newCommand.Left != CommandType.none)
        {
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            if (newCommand.IsSpecial)
            {
                commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
            }
            else
            {
                commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;
            commandObject.transform.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));
        }
        else
        {
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;
            commandObject.transform.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));
        }

        //commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;

        if (newCommand.Right != CommandType.none)
        {
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonSprite;
        }
        else
        {
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonSprite;
        }
    }


    void Start()
    {
        List = new List<Command>();

        _commandPrefab = Resources.Load<GameObject>("Prefabs/Command");
        _buttonASprite = Resources.Load<Sprite>("Sprites/ButtonA");
        _buttonBSprite = Resources.Load<Sprite>("Sprites/ButtonB");
        _arrowSprite = Resources.Load<Sprite>("Sprites/Arrow");


        for (int i = 0; i < 8; i++)
        {
            if (i%2 != 0)
            {
                Add(new Command());
            }
            else
            {
                AddRandomCommand();
            }
        }

        CommandIndex = 0;

        //_updateCommandDemand = true;
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            //UpdateUnflashSprite(child);
            if (child.name == ((8 + CommandIndex) % 8).ToString())
            {
                child.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                child.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            }
            UpdateUnflashSprite(child);

        }
        if(timeManager.flag)
        {
            UpdateFlashSprite(transform.GetChild(CommandIndex));
        }
        UpdateSprites();
    }

    void UpdateFlashSprite(Transform child)
    {
        child.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
        child.GetChild(1).GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    void UpdateUnflashSprite(Transform child)
    {
        child.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        child.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
    }

    public Command getRandomCommand()
    {
        int Length = _valuesCommandTypes.Length - 1;
        CommandType randomRight = (CommandType) _valuesCommandTypes.GetValue(_random.Next(2));
        CommandType randomLeft = (CommandType)_valuesCommandTypes.GetValue(_random.Next(4));
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

        //if (result && !timeManager.flag)
        //{
        //    result = false;
        //}

        _currentResult = result;
    }

    public void OnTimerChangeEvent(object sender, EventArgs e)
    {
        _isCurrentRunSuccessful = _currentResult;
        
        if (List[CommandIndex].Left != CommandType.none && List[CommandIndex].Right != CommandType.none)
        {
            if (ResolveCommandEventHandler != null)
            {
                ResolveCommandEventHandler.Invoke(this, new ResolveCommandEventArgs()
                {
                    IsCorrect = _currentResult,
                });
            }
        }
        Next();
        _isCurrentRunSuccessful = false;
        _currentResult = false;
    }

    public void Add(Command command)
    {
        List.Add(command);
        GetNewCommandObject(command);
    }

    public void AddRandomCommand()
    {
        Add(getRandomCommand());
    }


    private int howMany = 0;
    private int howManyToChange = 4;
    public float CHANCE_TO_RANDOM;
    public void Next()
    {
        howMany++;
        var rnd = new System.Random();
        if (CommandIndex % 2 == 0 )
        {
            if (rnd.NextDouble() < CHANCE_TO_RANDOM)
            {
                //ReplaceCommandObject(new Command(getRandomCommand().Left, List[(8 + CommandIndex - 2) % 8].Right, true), (8 + CommandIndex - 1) % 8);
            }
        }

        if (CommandIndex % 2 == 1)
        {
            //ReplaceCommandObject(new Command(), CommandIndex);
        }

        if (!(howMany < howManyToChange))
        {
            //ReplaceCommandObject(getRandomCommand(), (CommandIndex + 4) % 8);
            ReplaceCommandObject(getRandomCommand(), (CommandIndex + 5) % 8);
            //ReplaceCommandObject(getRandomCommand(), (CommandIndex + 6) % 8);
            ReplaceCommandObject(getRandomCommand(), (CommandIndex + 7) % 8);
            howMany = 0;
        }
        
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
