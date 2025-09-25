using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    #region Variables
    NavMeshAgent agent;
    [SerializeField] Transform[] waypoint;
    [SerializeField] int nextWaypoint;
    [SerializeField] float idleTimer = 2;
    [SerializeField] Transform player;

    bool hasdDetectedPlayer;

    EnemyPatrolState currentState;

    #endregion

    #region State Machine
    public enum EnemyPatrolState
    {
        Idle,
        Walking,
        Chase
    }
    #endregion

    #region Awake & Start
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        ChangeEnemyState(EnemyPatrolState.Walking);
    }
    #endregion

    #region Update
    private void Update()
    {
        #region Estados
        switch (currentState)
        {
            #region Idle State
            case EnemyPatrolState.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer < 0)
                {
                    idleTimer = 2;
                    ChangeEnemyState(EnemyPatrolState.Walking);
                }
                break;
            #endregion

            #region Walking State
            case EnemyPatrolState.Walking:
                if (hasdDetectedPlayer)
                {
                    ChangeEnemyState(EnemyPatrolState.Chase);
                }

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    Debug.Log("Llegue al waypoint" + nextWaypoint);

                    if (nextWaypoint > waypoint.Length)
                    {
                        nextWaypoint = 0;
                    }
                    else
                    {
                        nextWaypoint++;
                    }

                    Debug.Log("El siguiente waypoint es el" + nextWaypoint);
                    ChangeEnemyState(EnemyPatrolState.Idle);
                }
                break;
            #endregion

            #region Chase State
            case EnemyPatrolState.Chase:
                if (hasdDetectedPlayer)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    ChangeEnemyState(EnemyPatrolState.Idle);
                }
                break;
                #endregion
        }
        #endregion

        if (hasdDetectedPlayer)
        {
            ChangeEnemyState(EnemyPatrolState.Chase);
        }
    }
    #endregion

    void ChangeEnemyState(EnemyPatrolState newState)
    {
        currentState = newState;
        if (currentState == EnemyPatrolState.Walking)
        {
            agent.SetDestination(waypoint[nextWaypoint].position);
        }
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            hasdDetectedPlayer = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            hasdDetectedPlayer = false;
            player = null; 
        }
    }
    #endregion
}
