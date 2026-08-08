using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCWaypointMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float waypointReachDistance = 1.2f;

    [Header("Animator")]
    [SerializeField] private string speedParameter = "Speed";

    private Transform[] waypoints;
    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointIndex;
    private bool hasPath;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat(speedParameter, agent.velocity.magnitude);

        if (!hasPath || agent.pathPending || agent.isStopped)
        {
            return;
        }

        if (agent.remainingDistance <= waypointReachDistance)
        {
            GoToNextWaypoint();
        }
    }

    public void SetPath(Transform[] newWaypoints)
    {
        if (newWaypoints == null || newWaypoints.Length == 0)
        {
            Debug.LogWarning($"{name}: No path was assigned.");
            return;
        }

        waypoints = newWaypoints;
        currentWaypointIndex = 0;
        hasPath = true;

        MoveToCurrentWaypoint();
    }

    private void MoveToCurrentWaypoint()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"{name}: Agent is not on a NavMesh.");
            return;
        }

        Transform waypoint = waypoints[currentWaypointIndex];

        if (waypoint != null)
        {
            agent.isStopped = false;
            agent.SetDestination(waypoint.position);
        }
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            hasPath = false;
            agent.ResetPath();
            animator.SetFloat(speedParameter, 0f);
            return;
        }

        MoveToCurrentWaypoint();
    }
}