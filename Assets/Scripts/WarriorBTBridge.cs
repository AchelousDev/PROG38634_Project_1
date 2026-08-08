using UnityEngine;
using UnityEngine.AI;

public class WarriorBTBridge : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FindAndChaseMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        if (monsters.Length == 0)
        {
            if (agent.hasPath)
                agent.ResetPath();

            return;
        }

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distance =
                Vector3.Distance(transform.position, monster.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = monster;
            }
        }

        if (closest != null)
        {
            agent.SetDestination(closest.transform.position);
        }
    }
}