using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : EnemyController
{
    //how many points there should be
    public int totalPoints;
    //points to go between
    public List<Vector3> patrolPoints;
    public int currPoint;
    // Start is called before the first frame update
    void Start()
    {
        StartUp();

        GameManager.Instance.AddEnemy(this);
        pawn = this.GetComponent<NavMeshAgent>();

        //Sets pawns nav agent to the stats
        pawn.speed = TankStats.MoveSpeed;
        pawn.angularSpeed = TankStats.TurnSpeed;
        IsShooting = true; //Allows it to spam shots

        if(patrolPoints.Count < totalPoints)
        {
            InitializePatrol();
        }
    }
    //Sets up points if the amount isnt the total points set
    void InitializePatrol()
    {
        for (int i = patrolPoints.Count; i < totalPoints; i++)
        {
            //Gets a random point inside of a unit sphere and multiplies it by the radius of the idle radius
            Vector3 randomPoint = (Random.insideUnitSphere * idleRadius) + transform.position;

            //Makes the navmesh hit and then 
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, idleRadius, 1)) // Checks if it finds a point or not and stops if it doesn't
            {
                patrolPoints.Add(hit.position);
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        ArtificialIntelligence();

        Patrol();
    }
    private void Patrol()
    {
        switch (currentState)
        {
            case AIStates.Attack:
                pawn.isStopped = true;
                //How far it needs to rotate towards the player
                Quaternion neededRotation = Quaternion.LookRotation(playerLastPosition - this.transform.position);

                //Vector3 rotation = new Vector3(playerLastPosition.x, transform.rotation.y, playerLastPosition.z);
                transform.rotation = neededRotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * TankStats.TurnSpeed);
                //Debug.Log("Where looking" + neededRotation.eulerAngles);

                Shoot();
                break;
            case AIStates.Chase:
                pawn.SetDestination(playerLastPosition);

                NavMeshPath path = new NavMeshPath();
                pawn.CalculatePath(playerLastPosition, path);

                //if it isn't facing properly towards the path then it should try and turn before moving
                TurnTowards(path.corners[1]);
                break;
            case AIStates.ChooseTarget:
                //DetermineState();
                break;
            case AIStates.Flee:
                Debug.Log("Health Is Low");
                break;
            case AIStates.Idle:
                MoveToNextPoint();
                break;
        }
    }
    private void MoveToNextPoint()
    {
        //if tank is within 1 unit of point starts moving to next point
        float distance = Vector3.Distance(this.transform.position, patrolPoints[currPoint]);
        if(distance <= 1f)
        {
            //if point goes over then it should go back to zero
            currPoint = (currPoint + 1) % totalPoints;
        } else
        {
            pawn.SetDestination(patrolPoints[currPoint]);

            NavMeshPath path = new NavMeshPath();
            pawn.CalculatePath(patrolPoints[currPoint], path);
            //if it isn't facing properly towards the path then it should try and turn before moving
            TurnTowards(path.corners[1]);
        }
    }
}
