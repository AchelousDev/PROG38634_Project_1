using UnityEngine;
using UnityEngine.AI;

public class NavMeshAnimatorSync : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParameter = "Speed";

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (agent == null || animator == null)
        {
            return;
        }

        animator.SetFloat(speedParameter, agent.velocity.magnitude);
    }
}