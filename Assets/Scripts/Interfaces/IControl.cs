using UnityEngine;
using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;

public interface IControl
{
    event EventHandler PauseRequestEvent;
    void Update(GameState state);
    void SetControllable(IControllable controllable);
}
