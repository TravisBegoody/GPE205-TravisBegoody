using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShotgunEnemy : EnemyController
{
    // Update is called once per frame
    void Update()
    {
        ArtificialIntelligence();

        Shotgun();
    }
    //Fires with the shotgun
    private void Shotgun()
    {
        switch (currentState)
        {
            case AIStates.Attack:
                pawn.isStopped = true;
                //How far it needs to rotate towards the player
                Quaternion neededRotation = Quaternion.LookRotation(playerLastPosition - this.transform.position);

                transform.rotation = neededRotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * TankStats.TurnSpeed);
                //Debug.Log("Where looking" + neededRotation.eulerAngles);

                float temp = lastShot;
                //Shoots three bullets at the player
                Shoot(-15f);
                lastShot = temp;
                Shoot();
                lastShot = temp;
                Shoot(15f);
                break;
            case AIStates.Chase:
                pawn.isStopped = false;
                pawn.SetDestination(playerLastPosition);
                break;
            case AIStates.ChooseTarget:
                Debug.LogWarning("Enemy Controller Can't find a target");
                //DetermineState();
                break;
            case AIStates.Flee:
                float radius = 10f;
                Vector3 randomPosition = Random.insideUnitSphere * radius;

                //Makes the navmesh hit and then 
                if (randomPosition == null && NavMesh.SamplePosition(randomPosition, out fleePoint, radius, 1)) // Checks if it finds a point or not and stops if it doesn't
                {
                    pawn.SetDestination(fleePoint.position);
                }
                break;
            case AIStates.Idle:
                Debug.Log("Enemy is idle");
                break;
        }
    }
}
