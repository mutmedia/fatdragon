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

    private GameObject _commandPrefab;

    public Vector3 center;
    public float buttonOffset = 1;

    private Sprite _buttonASprite;
    private Sprite _buttonBSprite;
    private Sprite _arrowSprite;

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
        commandObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _arrowSprite;
        commandObject.transform.GetChild(0).Rotate(new Vector3(0, 0, angleLeft));
        commandObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonSprite;
    }
    
    void Start()
    {
        List = new ArrayList();

        _commandPrefab = Resources.Load<GameObject>("Prefabs/Command");
        _buttonASprite = Resources.Load<Sprite>("Sprites/ButtonA");
        _buttonBSprite = Resources.Load<Sprite>("Sprites/ButtonB");
        _arrowSprite = Resources.Load<Sprite>("Sprites/Arrow");


        for (int i = 0; i < 8; i++)
        {
            AddRandomCommand();
        }

        CommandIndex = 0;

        //_updateCommandDemand = true;
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            //UpdateUnflashSprite(child);
            if (child.name == CommandIndex.ToString())
            {
                child.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                child.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            }

        }
        if(timeManager.flag)
        {
            //UpdateFlashSprite(transform.GetChild(CommandIndex));
        }
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
        CommandType randomRight = (CommandType)_valuesCommandTypes.GetValue(_random.Next(Length - 2));
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
    }

    public void OnTimerChangeEvent(object sender, EventArgs e)
    {
        _isCurrentRunSuccessful = _currentResult;
        if (ResolveCommandEventHandler != null)
        {
            ResolveCommandEventHandler.Invoke(this, new ResolveCommandEventArgs()
            {
                IsCorrect = _currentResult,
            });
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
