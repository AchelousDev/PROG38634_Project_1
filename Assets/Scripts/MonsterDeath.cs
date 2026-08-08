using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDeath : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] private string deathTrigger = "Death";
    [SerializeField] private float destroyDelay = 2f;

    private Animator animator;
    private NavMeshAgent agent;
    private NPCCombatController combatController;
    private NPCWaypointMovement waypointMovement;
    private Collider monsterCollider;

    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();
        waypointMovement = GetComponent<NPCWaypointMovement>();
        monsterCollider = GetComponent<Collider>();
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.AddMonsterKill();
        }

        // Stop movement
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        // Stop AI
        if (combatController != null)
        {
            combatController.enabled = false;
        }

        if (waypointMovement != null)
        {
            waypointMovement.enabled = false;
        }

        // Prevent the dead monster from being hit again
        if (monsterCollider != null)
        {
            monsterCollider.enabled = false;
        }

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger(deathTrigger);
        }

        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}