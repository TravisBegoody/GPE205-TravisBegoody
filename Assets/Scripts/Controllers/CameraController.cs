using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 Offset;

    public int playerID;// Player in list it will follow
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        //How high it should be above player
        Offset = new Vector3(0f, this.transform.position.y, 0f);

        if(GameManager.Instance != null)
        {
            Player = GameManager.Instance.GetPlayer(playerID).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Player.transform.position + Offset;
    }
}
