using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCCombatController : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField] private string enemyTag;
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float attackRange = 1.8f;

    [Header("Combat")]
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Animator")]
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private string attackParameter = "Attack";

    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;
    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            FindTarget();
            UpdateMovementAnimation();
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance > detectionRadius * 1.5f)
        {
            currentTarget = null;
            return;
        }

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();

            FaceTarget();

            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger(attackParameter);
                nextAttackTime = Time.time + attackInterval;
            }
        }

        UpdateMovementAnimation();
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= detectionRadius && distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = enemy.transform;
            }
        }

        currentTarget = closestTarget;
    }

    private void FaceTarget()
    {
        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateMovementAnimation()
    {
        animator.SetFloat(speedParameter, agent.velocity.magnitude);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}