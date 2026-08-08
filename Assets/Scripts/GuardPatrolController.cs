using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GuardPatrolController : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 2f;
    [SerializeField] private float reachDistance = 1.2f;

    [Header("Animator")]
    [SerializeField] private string speedParameter = "Speed";

    private NavMeshAgent agent;
    private Animator animator;

    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning($"{name}: No patrol points assigned.");
            enabled = false;
            return;
        }

        MoveToCurrentPoint();
    }

    private void Update()
    {
        animator.SetFloat(speedParameter, agent.velocity.magnitude);

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                isWaiting = false;
                waitTimer = 0f;

                GoToNextPoint();
            }

            return;
        }

        if (agent.pathPending)
        {
            return;
        }

        if (agent.remainingDistance <= reachDistance)
        {
            agent.ResetPath();
            isWaiting = true;
        }
    }

    private void MoveToCurrentPoint()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"{name}: Guard is not on NavMesh.");
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    private void GoToNextPoint()
    {
        currentPointIndex++;

        if (currentPointIndex >= patrolPoints.Length)
        {
            currentPointIndex = 0;
        }

        MoveToCurrentPoint();
    }
}