using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    // AI behaviour stats, and navmesh
    public NavMeshAgent navMeshAgent;  // Nav Mesh Agent Component(Maps the floor and allows for movement)
    public float startWaitTime = 4;    // Time between actions
    public float timeToRotate = 2;    // Time for when the enemy detects an enemy without seeing them
    public float speedWalk = 6;       // Movement speeds in the navmesh agent
    public float speedRun = 9;       

    // Viewing angles
    public float viewRadius = 15;     // Radius of enemy view
    public float viewAngle = 90;     // Angle of enemy view

    // Collision detection
    public LayerMask playerMask;    // Layer masks to determine what gets hit in a raycast, enviroment or player
    public LayerMask obstacleMask;

    // Navmesh specifics
    public float meshResolution = 1f; // How many raycast are done per degree
    public int edgeIterations = 4;     // Amount of iterations of the cast, optimizes raycasts
    public float edgeDistance = 0.5f;   // Used to calculate the minimum and maximum distances of raycasts

    // Waypoints the AI will move to
    public Transform[] waypoints;       // All waypoints the enemy will patrol to
    int m_CurrentWaypointIndex;         // Index of waypoints

    // Player Info
    Vector3 playerLastPosition = Vector3.zero;      // Last position of the player when near the enemy
    Vector3 m_PlayerPosition;                       // Last position of seen player

    float m_WaitTime;               // Var to modify wait time
    float m_TimeToRotate;           // var to modify rotate speed
    bool m_PlayerInRange;           // if player is view, chase
    bool m_PlayerNear;              // if player is near
    bool m_IsPatrol;                // is patrolling or not
    bool m_CaughtPlayer;            // if the enemy caught the player

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;         // Initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); // Sets destination of first waypoint
    }

    // Update is called once per frame
    void Update()
    {
        EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;   // Set  to false since the enemy knows the player is close
        playerLastPosition = Vector3.zero;

        if(!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition); // Set  destination of the player location
        }

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) // Checks if the enemy closed in on the player position
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position,
                GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                // Checks if enemy is not near the player, returns to patrol after the waiting time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
    private void Patroling()
    {
        if(m_PlayerNear)
        {
            // If the enemy detects the player, the enemy will move towards that position
            if(m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;       // Player is not near when patrolling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // If the enemy reaches the waypoint, move to the next
                if(m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_TimeToRotate -= Time.deltaTime;
                }
            }
        }
    }


    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);

        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for(int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position); // Distance between player and enemy

                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if(Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }

            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
        
    }
}
