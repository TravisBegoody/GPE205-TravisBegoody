using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    // Start is called before the first frame update
    void Start()
    {
        StartUp();
        GameManager.Instance.AddEnemy(this);

        IsShooting = true; //Allows it to spam shots
    }

    // Update is called once per frame
    void Update()
    {
        ArtificialIntelligence();
    }
    /// <summary>
    /// Used to check if the Enemy should do any type of action depending on what is happening.
    /// </summary>
    void ArtificialIntelligence()
    {
        Shoot();
        //should have a calculation to check if player is in direction or aimed at player to shoot
        //should be able to navigate map
    }
    public override void Death()
    {
        GameManager.Instance.RemoveEnemy(this);

        gameObject.SetActive(false);
    }
}
