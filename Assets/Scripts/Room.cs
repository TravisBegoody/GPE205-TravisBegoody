using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour 
{
    public GameObject NorthDoor;
    public GameObject EastDoor; 
    public GameObject SouthDoor;
    public GameObject WestDoor;

    public List<GameObject> EnemySpawners = new List<GameObject>();
    public List<GameObject> PowerUpSpanwers = new List<GameObject>();
}
