using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WanderBotFSM : MonoBehaviour
{
    private enum State
    {
        Wander,
        Seek
    }

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Wander Settings")]
    [SerializeField] private float wanderRadius = 8f;
    [SerializeField] private float wanderInterval = 3f;

    [Header("Boundary Settings")]
    [SerializeField] private float maxDistanceFromOrigin = 20f;
    [SerializeField] private float returnRadius = 6f;

    private NavMeshAgent agent;
    private State currentState = State.Wander;

    private Vector3 originPosition;
    private float wanderTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        originPosition = transform.position;
        wanderTimer = 0f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Wander:
                UpdateWander();
                break;

            case State.Seek:
                UpdateSeek();
                break;
        }

        bool isWalking = agent.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("IsWalking", isWalking);
    }

    private void UpdateWander()
    {
        float distanceFromOrigin =
            Vector3.Distance(transform.position, originPosition);

        if (distanceFromOrigin > maxDistanceFromOrigin)
        {
            EnterSeekState();
            return;
        }

        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f &&
            !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomWanderDestination();
            wanderTimer = wanderInterval;
        }
    }

    private void UpdateSeek()
    {
        if (agent.pathPending)
        {
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = State.Wander;
            wanderTimer = 0f;
        }
    }

    private void EnterSeekState()
    {
        currentState = State.Seek;

        Vector3 randomPointInsideOrigin =
            originPosition + Random.insideUnitSphere * returnRadius;

        randomPointInsideOrigin.y = originPosition.y;

        if (NavMesh.SamplePosition(
                randomPointInsideOrigin,
                out NavMeshHit hit,
                returnRadius,
                NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(originPosition);
        }
    }

    private void SetRandomWanderDestination()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * wanderRadius;

        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        if (NavMesh.SamplePosition(
                randomDirection,
                out NavMeshHit hit,
                wanderRadius,
                NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}