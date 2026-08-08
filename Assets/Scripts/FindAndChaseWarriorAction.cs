using UnityEngine;
using UnityEngine.AI;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Custom/FindAndChaseWarrior")]
[Help("Finds the closest BattleWarrior, chases it, and attacks when in range.")]
public class FindAndChaseWarriorAction : GOAction
{
    private const float AttackRange = 3f;
    private const float AttackInterval = 3f;
    private const float RotationSpeed = 8f;

    private float lastAttackTime = -999f;

    public override TaskStatus OnUpdate()
    {
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        Animator animator = gameObject.GetComponentInChildren<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Monster BT cannot find Animator on " + gameObject.name);
        }

        if (agent == null)
        {
            return TaskStatus.FAILED;
        }

        GameObject[] warriors =
            GameObject.FindGameObjectsWithTag("BattleWarrior");

        if (warriors.Length == 0)
        {
            if (agent.hasPath)
            {
                agent.ResetPath();
            }

            return TaskStatus.COMPLETED;
        }

        GameObject closestWarrior = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject warrior in warriors)
        {
            if (warrior == null)
            {
                continue;
            }

            float distance = Vector3.Distance(
                gameObject.transform.position,
                warrior.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWarrior = warrior;
            }
        }

        if (closestWarrior == null)
        {
            return TaskStatus.COMPLETED;
        }

        // Target is outside attack range -> chase it.
        if (closestDistance > AttackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(closestWarrior.transform.position);

            return TaskStatus.COMPLETED;
        }

        // Target is inside attack range -> stop moving.
        agent.isStopped = true;

        if (agent.hasPath)
        {
            agent.ResetPath();
        }

        FaceTarget(closestWarrior.transform);

        // Play attack animation at intervals.
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            bool isAttacking = stateInfo.IsName("Attack");

            if (!isAttacking &&
                Time.time >= lastAttackTime + AttackInterval)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
            }
        }

        return TaskStatus.COMPLETED;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction =
            target.position - gameObject.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        gameObject.transform.rotation =
            Quaternion.Slerp(
                gameObject.transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );
    }
}