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

    public GameObject highscorePanel;

    public Animator DragonAnimator;

    public List<int> highscore;


    public int life = 3;
    public int missMaxNumber = 5;
    private int misses = 0;

    public CommandList TheCommandList;

    public GameState state = GameState.Playing;
    public GameState lastState;
    private float unpauseTime;

    public EventHandler<EndGameEventArgs> EndGameEventHandler;

    public void buildHighscore(object sende, EventArgs e)
    {
        highscorePanel.SetActive(true);
        int BasePoints = 10000;
   
        highscorePanel.GetComponent<Transform>().GetChild(10).GetComponent<Text>().text = scoreManager._score.ToString();

        int i = 0;
        foreach(Transform child in highscorePanel.GetComponent<Transform>())
        {
            if(i < 10)
            {
                BasePoints += 10000 * (i + 1);

                if (scoreManager._score >= BasePoints && scoreManager._score <= 10000 * (i + 2) + BasePoints)
                {
                    child.GetComponent<Text>().text = (10-i)+". Player";
                    child.GetChild(0).GetComponent<Text>().text = scoreManager._score.ToString();
                }
                else
                {
                    child.GetChild(0).GetComponent<Text>().text = BasePoints + "";
                }

                i++;
            }
            
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
                    player.EndAnimationFinishedEventHandler = buildHighscore;
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
                        DragonAnimator.SetTrigger("PlayerMisses");
                        Debug.Log("Life " + life);
                        break;
                    case 1:
                        DragonAnimator.SetTrigger("PlayerMissesSmokes");
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
