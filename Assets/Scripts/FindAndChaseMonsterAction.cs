using UnityEngine;
using UnityEngine.AI;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Custom/FindAndChaseMonster")]
[Help("Finds the closest active Monster and moves this BattleWarrior toward it.")]
public class FindAndChaseMonsterAction : GOAction
{
    public override TaskStatus OnUpdate()
    {
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogWarning("FindAndChaseMonster: NavMeshAgent not found on " + gameObject.name);
            return TaskStatus.FAILED;
        }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        if (monsters.Length == 0)
        {
            if (agent.hasPath)
            {
                agent.ResetPath();
            }

            // No monster right now is not an error.
            // Finish this action and let Repeat try again later.
            return TaskStatus.COMPLETED;
        }

        GameObject closestMonster = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            if (monster == null)
            {
                continue;
            }

            float distance = Vector3.SqrMagnitude(
                monster.transform.position - gameObject.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMonster = monster;
            }
        }

        if (closestMonster != null)
        {
            agent.SetDestination(closestMonster.transform.position);
        }

        return TaskStatus.COMPLETED;
    }
}