using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AI_Logic : MonoBehaviour
{
    // For moving the AI 
    [SerializeField] private NavMeshAgent agent;
    [Range(1, 500)] public float walkRadius;

    // For shooting
    private FirePoint firePoint;
    public bool canAttack;

    // For player vision
    [SerializeField] private float viewRadius;
    [SerializeField, Range(1, 360)] private float viewAngles;
    [SerializeField] private LayerMask targetMask, obstacleMask;

    // To see if the player has a bullet or not
    [SerializeField] private Light light;

    // For States
    private enum State
    {
        wander,
        attack,
        evade,
        chase,
    }

    private State state;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        firePoint = GetComponentInChildren<FirePoint>();
        light = GetComponent<Light>();
        
        // Tells the player to go to random direction
        agent.SetDestination(GetRandomLocation());
    }

    void Update()
    {
        switch (state)
        {
            case State.wander:
                Wander();
                EnemyDetection();
                break;
            
            case State.attack:
                Attack();
                break;
            
            case State.evade:
                if (canAttack)
                {
                    state = State.chase;
                }
                else
                {
                    light.enabled = false;
                    Evade();
                }
                break;
            
            case State.chase:
                light.enabled = true;
                Chase();
                break;
        }
    }

    public void HasReloaded()
    {
        canAttack = true;
        print(name + " has reloaded");
    }

    private Vector3 lastKnownEnemyPos;

    // Wander State
    private void Wander()
    {
        // Check if it reached the location, then set a new location
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(GetRandomLocation());
        }
    }
    
    // Gives a random location so the AI wanders
    private Vector3 GetRandomLocation()
    {
        // Debug.Log("wander");
        Vector3 randomLocation = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        
        // Finds the nearest point based on the NavMesh within a specified range of walkRadius
        if ( randomPosition != Vector3.zero && NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            randomLocation = hit.position;
            // Debug.Log(randomLocation);
        }

        return randomLocation;
    }
    
    // AI line of vision
    private void EnemyDetection()
    {
        // Make a collider around the player which sees other players
        // Here it fails if all of the players have the same layerMask, so different layerMask are needed for each different player
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float distToTarget = Vector3.Distance(transform.position, target.position);
            
            // Debug.DrawRay(transform.position, dirToTarget * 10, Color.red);
            
            // Cast a cone for the player vision, limiting what they can see to things inside of the cone
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngles / 2)
            {
                // Cast a line from the player to the target to see if they're behind a wall or not
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    var rotateDirection = (target.position - transform.position).normalized;
                    var targetRotation = Quaternion.LookRotation(rotateDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);

                    // For chase state
                    lastKnownEnemyPos = target.transform.position;
                    state = State.attack;
                }   
            }
        }
    }
    
    private Vector3 DirFromAngle(float angleInDegress, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegress += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }

    // To check the radius of players
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkRadius);

        Gizmos.color = Color.red;

        Vector3 viewAngleA = DirFromAngle(-viewAngles / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngles / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }
    
    

    // Attack State
    private void Attack()
    {
        canAttack = false;
        firePoint.Shoot();
        state = State.evade;
    }
    
    // Evade State
    private void Evade()
    {
        // Get a list of all the bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        if (bullets.Length == 0)
        {
            state = State.wander;

            return;
        }
        
        GameObject bullet = bullets[0];
        
        // Get a list of all the players
        // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // Currently it bugs out if all the players die, can maybe fix with using ^ and excluding itself from the list. Still works though.
        GameObject[] moreThanOnePlayer = GameObject.FindGameObjectsWithTag("Player");
        Collider[] players = null;
        
        if (moreThanOnePlayer.Length > 1)
        {
            players = Physics.OverlapSphere(transform.position, walkRadius, targetMask);

            if (players.Length == 0)
            {
                state = State.wander;

                return;
            }
        }

        Transform player = players[0].transform;
        
        // Set the Evasion distance
        float evasionDistance = 5f;

        // Closest obj to dodge
        Vector3 closestObj;

        // Determines the closest bullet
        for (int i = 0; i < bullets.Length; i++)
        {
            if (Vector3.Distance(transform.position, bullet.transform.position) > 
                Vector3.Distance(transform.position, bullets[i].transform.position))
            {
                bullet = bullets[i];
            }
        }
    
        // Determines the closest player
        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(transform.position, player.position) > 
                Vector3.Distance(transform.position, players[i].transform.position))
            {
                player = players[i].transform;
                
                // for chase state
                lastKnownEnemyPos = player.transform.position;
            }
        }

        //Checks if the bullet is closer than the player
        if (Vector3.Distance(transform.position, bullet.transform.position) >
            Vector3.Distance(transform.position, player.position))
        {
            closestObj = player.position;
        }
        else
        {
            closestObj = bullet.transform.position;
        }

        // Find current bullets velocity, adjust current velocity to swerve away from
        // Calculate the direction to evade the bullet
        Vector3 evadeDirection = transform.position - closestObj;
        evadeDirection.y = 0;
        evadeDirection.Normalize();
    
        // Move in the opposite direction of the bullet
        Vector3 evadeDir = transform.position + evadeDirection * evasionDistance;
        agent.SetDestination(evadeDir);
    }

    private void Chase()
    {
        agent.SetDestination(lastKnownEnemyPos);
        
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            state = State.wander;
        }
    }

    private bool lastManStanding;

    public void IsLastManStanding()
    {
        print(name + " is winner");
        lastManStanding = true;
        
        state = State.wander;
    }

    // Kills the AI
    public void Die()
    {
        // print("hit");
        // Can't use destroy, as it would break ammoController
        // Destroy(gameObject);
        
        // So we deactivate it instead
        if (!lastManStanding)
        {
            gameObject.SetActive(false);
        }
    }
}
