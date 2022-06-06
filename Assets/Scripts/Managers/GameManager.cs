using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private List<PlayerController> Players = new List<PlayerController>();
    private List<EnemyController> Enemys = new List<EnemyController>();
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPlayer(PlayerController player)
    {
        Players.Add(player);
    }
    public void RemovePlayer(PlayerController player)
    {
        if (Players.Contains(player))
        {
            Players.Remove(player);
        }
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
}