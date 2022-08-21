using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterController
{
    public enum AIStates { Idle, Chase, Attack, Flee, ChooseTarget};
    public enum AISenses { See, Hear, Near, None };
    [SerializeField] protected AIStates currentState;
    [SerializeField] protected AISenses currentSense;
    private float timeInState;
    protected NavMeshAgent pawn;
    protected NavMeshHit fleePoint;

    //Used to investigate area where player was
    [SerializeField] protected Vector3 playerLastPosition;


    [SerializeField] protected float idleRadius;
    //is it currently idle moving
    protected bool idleMovement;

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
    }

    // Update is called once per frame
    void Update()
    {
        ArtificialIntelligence();

        //Used for the default enemy
        DefaultState();
    }
    /// <summary>
    /// Used to check if the Enemy should do any type of action depending on what is happening.
    /// </summary>
    public void ArtificialIntelligence()
    {
        TargetCheck();
        DetermineState();
        //should have a calculation to check if player is in direction or aimed at player to shoot
        //should be able to navigate map
    }
    public override void Death()
    {
        GameManager.Instance.RemoveEnemy(this);

        gameObject.SetActive(false);
    }
    public void MoveTowards(Transform target)
    {
        if(target != null)
        {
            pawn.SetDestination(target.position);
        }
    }
    private void DefaultState()
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
    public void DetermineState()
    {
        switch (currentSense)
        {
            case AISenses.See: // Enemy can see the player in their line of sight
                currentState = AIStates.Attack;
                break;
            case AISenses.Hear: // Enemy can hear the player
                currentState = AIStates.Chase;
                break;
            case AISenses.Near: // Enemy has no walls between them and player but not in line of sight
                currentState = AIStates.Attack;
                break;
            case AISenses.None: // Enemy can't see or hear player
                currentState = AIStates.Idle;
                break;
        } // Enemy is low on health
        if((currentHealth / TankStats.Health) * 100f <= 30f)
        {
            currentState = AIStates.Flee;
        }
    }
    //Checks if it can see or hear the player or any type of creature its against
    public void TargetCheck()
    {
        RaycastHit hit;

        Vector3 targetPosition = GameManager.Instance.GetPlayer1().transform.position;
        Vector3 AIPostion = this.transform.position;
        Vector3 direction = targetPosition - AIPostion;

        //Draws the line of sight for the tank
        float angle = Vector3.Angle(direction, transform.forward);

        currentSense = AISenses.None;

        //hears player
        if (Vector3.Distance(AIPostion, targetPosition) <= TankStats.HearRange)
        {
            currentSense = AISenses.Hear;
            playerLastPosition = targetPosition;
        }

        //Create a ray cast as far as sightrange, checks if it it the player, check if it is in 40 degrees of sight
        //Sees player
        if (Physics.Raycast(AIPostion, direction, out hit, TankStats.SightRange) && hit.collider.transform.position == targetPosition)
        {
            if (angle < TankStats.SightFOV)
            {
                currentSense = AISenses.See;
            }
        }
        //Debug.Log(hit.collider.gameObject.name);
        //Debugs line of sight of the player
        switch (currentSense)
        {
            case AISenses.See:
                Debug.DrawRay(AIPostion, direction, Color.green);
                break;
            case AISenses.Hear:
                Debug.DrawRay(AIPostion, direction, Color.yellow);
                break;
            case AISenses.Near:
                Debug.DrawRay(AIPostion, direction, Color.cyan);
                break;
            case AISenses.None:
                Debug.DrawRay(AIPostion, direction, Color.red);
                break;
        }
    }
    //Makes an enemy randomly move around
    public void IdleMovement()
    {
        if (!idleMovement)
        {
            //Gets a random point inside of a unit sphere and multiplies it by the radius of the idle radius
            Vector3 randomPoint = Random.insideUnitSphere * idleRadius;

            //Makes the navmesh hit and then 
            NavMeshHit hit;
            StartCoroutine(QuickBreak());
            if (NavMesh.SamplePosition(randomPoint, out hit, idleRadius, 1)) // Checks if it finds a point or not and stops if it doesn't
            {
                pawn.isStopped = false;
                pawn.SetDestination(hit.position + transform.position);
            }
        }
    }
    //Stops the pawn til it is facing towards the position
    public void TurnTowards(Vector3 position)
    {
        if (Vector3.Angle(this.transform.forward, position - this.transform.position) > 12f)
        {
            pawn.isStopped = true;

            Quaternion turn = Quaternion.LookRotation(position - this.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turn, Time.deltaTime * TankStats.TurnSpeed);
        }
        else
        {
            pawn.isStopped = false;
        }
    }
    public IEnumerator QuickBreak()
    {
        idleMovement = true;
        yield return new WaitForSeconds(5f);
        idleMovement = false;
    }

}
