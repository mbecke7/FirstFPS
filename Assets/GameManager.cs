using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private const string PLAYER_ID_PREFIX = "Player ";

    public delegate void OnPlayerKilledCallback (string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    public MatchSettings matchSettings;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;
    }

    #region Player tracking
    
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
    #endregion
}
