using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField]private List<PlayerController> Players = new List<PlayerController>();
    [SerializeField]private List<EnemyController> Enemys = new List<EnemyController>();
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void AddPlayer(PlayerController player)
    {
        Players.Add(player);
    }
    public PlayerController GetPlayer1() { return Players[0]; }
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
}