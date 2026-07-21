using UnityEngine;
using UnityEngine.AI;

public class PatrolBotFSM : MonoBehaviour
{
    public Transform[] waypoints;
    public Animator animator;

    private NavMeshAgent agent;

    private enum State
    {
        Patrol,
        Wait
    }

    private State currentState = State.Patrol;

    private int currentWaypoint = 0;
    private float waitTimer = 0f;

    public float waitTime = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GoToWaypoint(currentWaypoint);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolUpdate();
                break;

            case State.Wait:
                WaitUpdate();
                break;
        }

        animator.SetBool("IsWalking", agent.velocity.magnitude > 0.1f);
    }

    void PatrolUpdate()
    {
        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            if (currentWaypoint < waypoints.Length - 1)
            {
                currentWaypoint++;
                GoToWaypoint(currentWaypoint);
            }
            else
            {
                currentState = State.Wait;
                waitTimer = waitTime;
                agent.ResetPath();
            }
        }
    }

    void WaitUpdate()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0)
        {
            currentWaypoint = 0;
            GoToWaypoint(currentWaypoint);
            currentState = State.Patrol;
        }
    }

    void GoToWaypoint(int index)
    {
        agent.SetDestination(waypoints[index].position);
    }
    
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null)
            {
                continue;
            }

            Gizmos.DrawSphere(waypoints[i].position, 0.4f);

            if (i < waypoints.Length - 1 &&
                waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(
                    waypoints[i].position,
                    waypoints[i + 1].position
                );
            }
        }

        if (waypoints.Length > 1 &&
            waypoints[0] != null &&
            waypoints[waypoints.Length - 1] != null)
        {
            Gizmos.DrawLine(
                waypoints[waypoints.Length - 1].position,
                waypoints[0].position
            );
        }
    }
}