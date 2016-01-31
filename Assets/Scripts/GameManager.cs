using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts;
using UnityEngine.UI;

public class EndGameEventArgs : EventArgs
{
    public float timeOfDeath { get; set; }
}

public class GameManager : MonoBehaviour {

    public List<IControl> controllers;
    private PlayerManager playerManager;
    private ScoreManager scoreManager;
    private TimeManager timeManager;
    private SoundManager soundManager;

    public Transform highscorePanel;

    public Text playerScore;

    public Animator DragonAnimator;
    public GameObject FirebreathAnimator;

    public List<int> highscore;


    public int life = 3;
    public int missMaxNumber = 5;
    private int misses = 0;

    public CommandList TheCommandList;

    public GameState state = GameState.Playing;
    public GameState lastState;
    private float unpauseTime;

    public EventHandler<EndGameEventArgs> EndGameEventHandler;

    public void buildHighscore()
    {
        int BasePoints = 10000;
        int i;
        for(i = 0; i < 5;i++)
        {
            BasePoints *= 2 * i;
            highscore.Add(BasePoints);
        }
        for (i = 5; i < 9; i++)
        {
            BasePoints *= 5 * i;
            highscore.Add(BasePoints);
        }

        highscore.Add(BasePoints*10);

        playerScore.text = scoreManager._score.ToString();

        i = 0;
        foreach(Transform child in highscorePanel)
        {
            child.GetChild(0).GetComponent<Text>().text = highscore[i].ToString();
            i++;
        }
    }

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        timeManager = GetComponent<TimeManager>();
        scoreManager = GetComponent<ScoreManager>();
        soundManager = GetComponent<SoundManager>();
        TheCommandList.timeManager = timeManager;
    }

	// Use this for initialization
	void Start () {
        buildHighscore();
        controllers = new List<IControl>();

        XBoxJoystickControl.Reset();

        var noControlAvailable = false;

        for (int i = 0; i < 4 && !noControlAvailable; i++)
        {
            var control = XBoxJoystickControl.GetControl();
            if (control != null)
            {
                var player = playerManager.CreatePlayer();
                if (player != null)
                {
                    control.SetControllable(player);
                    controllers.Add(control);
                    control.PauseRequestEvent += OnPauseRequestEvent;
                    player.CommandEventHandler += OnNewPlayerCommand;
                    EndGameEventHandler = player.OnDeathResolve;
                    TheCommandList.Add(player);
                }
            }
            else
            {
                noControlAvailable = true;
                
            }
        }
        
        //More event Logic
        TheCommandList.ResolveCommandEventHandler += scoreManager.OnResolveCommand;
        TheCommandList.ResolveCommandEventHandler += OnResolveCommand ;
        TheCommandList.OnListOverEventHandler += scoreManager.OnListOver;
        timeManager.TimeNextCommandEventHandler += TheCommandList.OnTimerChangeEvent;
        // REMOVETHIS
        timeManager.StartCounting();

        //Sounds
	    TheCommandList.ResolveCommandEventHandler += soundManager.OnMistake;

	}

    public void OnResolveCommand(object sende, ResolveCommandEventArgs e)
    {
        if (!e.IsCorrect)
        {
            misses++;
            if (misses >= missMaxNumber)
            {
                misses = 0;
                life--;

                switch(life)
                {
                    case 2:
                        DragonAnimator.SetTrigger("PlayerMissesGurgle");
                        Debug.Log("Life " + life);
                        break;
                    case 1:
                        //DragonAnimator.SetTrigger("PlayerMissesSmoke");
                        Debug.Log("Life " + life);
                        break;
                    case 0:
                        DragonAnimator.SetTrigger("PlayerDies");
                        //FirebreathAnimator.SetActive(true);
                        timeManager.StopCounting();
                        Debug.Log("Dead! ");
                        if (EndGameEventHandler != null)
                        {
                            EndGameEventHandler.Invoke(this, new EndGameEventArgs()
                            {
                                timeOfDeath = Time.time,
                            });
                        }
                        life = 3;
                        break;
                }
            }
        }
    }


    private void OnPauseRequestEvent(object sender, EventArgs e)
    {
        if (Time.realtimeSinceStartup < unpauseTime) return;

        if (state == GameState.Paused)
        {
            Time.timeScale = 1.0f;
            state = lastState;
            ChangeState(GameState.Paused);
        }

        else
        {
            Time.timeScale = 0.0f;
            lastState = state;
            ChangeState(GameState.Paused);
        }
        unpauseTime = Time.realtimeSinceStartup + 0.5f;
        Debug.Log("Pause requested");

    }

    public void ChangeState(GameState newState)
    {
        state = newState;
    }


	// Update is called once per frame
	void Update() {
        foreach (IControl c in controllers)
        {
            c.Update(state);
        }
	}

    private int commandsSent = 0;
    void OnNewPlayerCommand(object sender, CommandEventArgs e)
    {
        //Debug.Log(commandsSent + ":Player just sent the command " + e.Command.Left + " - " + e.Command.Right);

        commandsSent++;
    }
}
