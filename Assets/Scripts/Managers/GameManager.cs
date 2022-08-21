using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MapGenerator Map;
    public int seed;
    [Range(1,10)]
    public int XAxis, YAxis;

    [SerializeField] private List<PlayerController> Players = new List<PlayerController>();
    [SerializeField] private List<EnemyController> Enemys = new List<EnemyController>();

    public GameState CurrentState;
    /*private void OnValidate()
    {
        Debug.Log("Update");
        if (Map == null)
        {
            return;
        }
        if(Map.grid != null)
        {
            Map.UpdateMap(seed);
        } else
        {
            Map.GenerateMap(XAxis,YAxis,seed);
        }
    }*/
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    private void Start()
    {
        Map.GenerateMap(XAxis, YAxis, seed);
    }
    public void StartDaily()
    {
        Map.DailyLevel();
    }
    public void StartUpdate()
    {
        Debug.Log("Started update");
        //Map.UpdateMap(seed);
    }
    public void AddMap(MapGenerator map)
    {
        Map = map;
    }
    public void AddPlayer(PlayerController player)
    {
        Players.Add(player);
    }
    public PlayerController GetPlayer1()
    {
        if(Players.Count == 0)
        {
            throw new Exception("No Players Exist");
        }
        return Players[0];
    }
    public void RemovePlayer(PlayerController player)
    {
        if (Players.Contains(player))
        {
            Players.Remove(player);
        }
    }
    //Checks if the position is a player
    public bool IsPlayer(Vector3 position)
    {
        foreach(PlayerController player in Players)
        {
            if(player.transform.position == position)
            {
                return true;
            }
        }
        return false;
    }
    public void AddEnemy(EnemyController enemy)
    {
        Enemys.Add(enemy);
    }
    public void RemoveEnemy(EnemyController enemy)
    {
        if (Enemys.Contains(enemy))
        {
            Enemys.Remove(enemy);
        }
    }
    public bool IsEnemy(Vector3 position)
    {
        foreach (EnemyController enemy in Enemys)
        {
            if (enemy.transform.position == position)
            {
                return true;
            }
        }
        return false;
    }
    public void ChangeGameState(GameState state)
    {
        CurrentState = state;

        switch(CurrentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f; 
                
                foreach (PlayerController player in Players) { player.ChangeInput(true); }
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.NoPlayer:
                foreach(PlayerController player in Players) { player.ChangeInput(false); }
                break;
        }
    }
    public enum GameState
    {
        Playing,
        Paused,
        NoPlayer,
    }
}