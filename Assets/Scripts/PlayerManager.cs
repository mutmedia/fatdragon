using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _startPositionList;

    private List<Player> players;
    
    void Awake()
    {
        players = new List<Player>();
    }

    public Player CreatePlayer()
    {
        var player = Player.Create(_startPositionList[players.Count].position, players.Count);
        players.Add(player);
        return player;
    }

    public List<Player> GetPlayerList()
    {
        return players;
    }
}