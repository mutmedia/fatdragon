using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;

public class GameManager : MonoBehaviour {

    public List<IControl> controllers;
    private PlayerManager playerManager;

    public GameState state = GameState.Playing;
    public GameState lastState;
    private float unpauseTime;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

	// Use this for initialization
	void Start () {
        controllers = new List<IControl>();

        XBoxJoystickControl.Reset();

        var noControlAvailable = false;

        for (int i = 0; i < 4 && !noControlAvailable; i++)
        {
            var control = XBoxJoystickControl.GetControl();
            Debug.Log("Controller " + i + " = " + control);
            if (control != null)
            {
                var player = playerManager.CreatePlayer();
                if (player != null)
                {
                    control.SetControllable(player);
                    controllers.Add(control);
                    control.PauseRequestEvent += OnPauseRequestEvent;
                }
            }
            else
            {
                noControlAvailable = true;
                
            }
        }

        //Monster logic
        int numberOfControllers = Input.GetJoystickNames().Length;
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
}
