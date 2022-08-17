using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeEnemy : EnemyController
{
    private bool detonationSet;
    // Update is called once per frame
    void Update()
    {
        ArtificialIntelligence();

        Kamikaze();
    }
    private void Kamikaze()
    {
        switch (currentState)
        {
            case AIStates.Attack: //Is within exploding range and starts to explode
                pawn.isStopped = true;
                //How far it needs to rotate towards the player
                Quaternion neededRotation = Quaternion.LookRotation(playerLastPosition - this.transform.position);

                transform.rotation = neededRotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * TankStats.TurnSpeed);
                //Debug.Log("Where looking" + neededRotation.eulerAngles);

                //If it is close to the player then it sets itself to blow up
                if (!detonationSet)
                {
                    StartCoroutine(InitiateExplosion());
                }
                break;
            case AIStates.Chase:
                pawn.isStopped = false;
                pawn.SetDestination(playerLastPosition);
                break;
            case AIStates.ChooseTarget:
                goto case AIStates.Chase;
            case AIStates.Flee:
                goto case AIStates.Chase;
            case AIStates.Idle:
                //Enemy Should wander around

                IdleMovement();
                break;
        }
    }
    IEnumerator InitiateExplosion()
    {
        detonationSet = true;
        Debug.Log("IM GONNA MORB");

        yield return new WaitForSeconds(3f);
        Death();
    }
    public override void Death()
    {
        GameManager.Instance.RemoveEnemy(this);

        //Explodes
        Instantiate(TankStats.Projectile, this.transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
