using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isMapOfDay;
    public MapGenerator Map;
    public int seed;
    [Range(1,10)]
    public int XAxis, YAxis;

    [Range(0, 100)]
    public int EnemyChance;
    [Range(0, 100)]
    public int PowerChance;

    [SerializeField] private List<PlayerController> Players = new List<PlayerController>();
    [SerializeField] private List<EnemyController> Enemys = new List<EnemyController>();

    [SerializeField] private GameObject[] EnemyTypes;
    [SerializeField] private GameObject[] PowerTypes;

    private List<GameObject> PlayerSpawner = new List<GameObject>();
    private List<GameObject> EnemySpawner = new List<GameObject>();
    private List<GameObject> PowerSpawner = new List<GameObject>();



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
        if (isMapOfDay)
        {
            Map.DailyLevel();
        }
        else
        {
            Map.GenerateMap(XAxis, YAxis, seed);
        }
        StartCoroutine(Spawn());
    }
    #region Map
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
    public void AddEnemySpawner(GameObject position)
    {
        EnemySpawner.Add(position);
    }
    public void AddPowerSpawner(GameObject position)
    {
        PowerSpawner.Add(position);
    }
    public void AddPlayerSpawner(GameObject position)
    {
        PlayerSpawner.Add(position);
    }

    void SpawnEnemies()
    {
        var prng = new System.Random(seed);
        foreach (GameObject spawner in EnemySpawner)
        {
            int SpawnChance = prng.Next(0, 100);
            if (SpawnChance > EnemyChance)
            {
                int index = prng.Next(0, EnemyTypes.Length);
                Instantiate(EnemyTypes[index], spawner.transform.position, Quaternion.identity);
            }
        }
    }
    void SpawnPowerUps()
    {
        var prng = new System.Random(seed);
        foreach (GameObject spawner in PowerSpawner)
        {
            int SpawnChance = prng.Next(0, 100);
            if (SpawnChance > PowerChance) {
                int index = prng.Next(0, PowerTypes.Length);
                Instantiate(PowerTypes[index], spawner.transform.position, Quaternion.identity);
            }
        }

    }
    void SpawnPlayer()
    {
        var prng = new System.Random(seed);
        foreach(PlayerController player in Players)
        {
            int spawnPos = prng.Next(0, PlayerSpawner.Count);

            player.transform.position = PlayerSpawner[spawnPos].transform.position;
        }
    }
    #endregion
    #region Player
    public void AddPlayer(PlayerController player)
    {
        Players.Add(player);
    }
    public PlayerController GetPlayer(int index)
    {
        if (Players.Count == 0)
        {
            throw new Exception("No Players Exist");
        }
        return Players[index];
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
    #endregion
    #region Enemy
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
    #endregion
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
    public void PlayerDeath()
    {
        SceneManager.LoadScene("Main");
    }
    public enum GameState
    {
        Playing,
        Paused,
        NoPlayer,
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);

        SpawnEnemies();
        SpawnPowerUps();
        SpawnPlayer();
    }
}