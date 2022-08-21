using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<Room> RoomTemplate = new List<Room>();

    int XLength, YLength;

    public Room[,] grid;

    private float roomWidth = 31f;
    private float roomHeight = 31f;

    public NavMeshSurface surface;

    void Start()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Map = this;
        }
    }
    public void GenerateMap(int sizex, int sizey, int seed)
    {
        DestroyMap();

        XLength = sizex;
        YLength = sizey;

        grid = new Room[sizex, sizey];

        UpdateMap(seed);
    }
    public void UpdateMap(int seed)
    {
        var prng = new System.Random(seed);

        for (int x = 0; x < XLength; x++)
        {
            for (int y = 0; y < YLength; y++)
            {
                int index = prng.Next(0, RoomTemplate.Count);

                //if not empty and template is not same 
                if (grid[x, y] != null && grid[x,y] != RoomTemplate[index])
                {
                    DestroyRoom(grid[x, y]);
                }

                Vector3 newPosition = new Vector3(x * roomWidth,
                                               0f,
                                               y * roomHeight);

                Room newRoom = Instantiate(RoomTemplate[index],newPosition,Quaternion.identity,this.transform) as Room;

                newRoom.gameObject.name = "Room " + "(" + x + ", " + y + ")";

                grid[x, y] = newRoom;
            }
        }

        //Determines if doors should be open or not
        for (int x = 0; x < XLength; x++)
        {
            for (int y = 0; y < YLength; y++)
            {
                //avoids communication with non existant 0 - 1 point and out of index .Length + 1
                if(x != 0) //Not West Edge
                {
                    grid[x, y].WestDoor.SetActive(false);
                }
                if(x != XLength - 1) //Not East Edge
                {
                    grid[x, y].EastDoor.SetActive(false);
                }
                if (y != 0)// Not South Edge
                {
                    grid[x, y].SouthDoor.SetActive(false);
                } 
                if(y != YLength - 1)// Not North Edge
                {
                    grid[x, y].NorthDoor.SetActive(false);
                }
            }
        }
        surface.BuildNavMesh();
    }
    public void DestroyMap()
    {
        var oldRooms = FindObjectsOfType<Room>();
        for (int i = oldRooms.Length - 1; i >= 0; i--)
        {
            DestroyRoom(oldRooms[i]);
        }
    }
    public void DestroyRoom(Room room)
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Destroyed " + room.name);
            StartCoroutine(Destroy(room.gameObject));
        }
        else
        {
            Destroy(room.gameObject);
        }
    }
    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }
    public void DailyLevel()
    {
        var date = DateTime.Today;
        int dailySeed = (date.Month * 1000000) + (date.Day * 10000) + (date.Year);
        Debug.Log("Day is " + dailySeed);
        GenerateMap(5,5,dailySeed);
    }
}
