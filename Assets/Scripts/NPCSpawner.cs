using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private string npcTag;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform npcParent;

    [Header("Wave Settings")]
    [SerializeField] private int groupSize = 3;
    [SerializeField] private int maxActiveNPCs = 3;
    [SerializeField] private float initialDelay = 0f;
    [SerializeField] private float respawnDelay = 8f;
    [SerializeField] private float delayBetweenSpawns = 0.4f;

    [Header("Spawn Position")]
    [SerializeField] private float spawnRadius = 1.5f;
    [SerializeField] private float navMeshSearchDistance = 4f;

    private bool waitingForRespawn = false;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        if (initialDelay > 0f)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        while (true)
        {
            int currentCount = CountActiveNPCs();

            if (currentCount < maxActiveNPCs && !waitingForRespawn)
            {
                waitingForRespawn = true;

                int availableSlots = maxActiveNPCs - currentCount;
                int amountToSpawn = Mathf.Min(groupSize, availableSlots);

                yield return StartCoroutine(SpawnGroup(amountToSpawn));

                yield return new WaitForSeconds(respawnDelay);

                waitingForRespawn = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SpawnGroup(int amount)
    {
        if (!ValidateSetup())
        {
            yield break;
        }

        for (int i = 0; i < amount; i++)
        {
            SpawnNPC();

            if (i < amount - 1)
            {
                yield return new WaitForSeconds(delayBetweenSpawns);
            }
        }
    }

    private void SpawnNPC()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

        Vector3 requestedPosition =
            transform.position +
            new Vector3(randomCircle.x, 0f, randomCircle.y);

        if (!NavMesh.SamplePosition(
                requestedPosition,
                out NavMeshHit hit,
                navMeshSearchDistance,
                NavMesh.AllAreas))
        {
            Debug.LogWarning(
                $"{name}: Could not find a valid NavMesh position near spawn point."
            );
            return;
        }

        GameObject npc = Instantiate(
            npcPrefab,
            hit.position,
            transform.rotation,
            npcParent
        );

        NPCWaypointMovement movement =
            npc.GetComponent<NPCWaypointMovement>();

        if (movement == null)
        {
            Debug.LogError(
                $"{npc.name}: NPCWaypointMovement component is missing."
            );

            Destroy(npc);
            return;
        }

        movement.SetPath(waypoints);
    }

    private int CountActiveNPCs()
    {
        if (string.IsNullOrWhiteSpace(npcTag))
        {
            return 0;
        }

        return GameObject.FindGameObjectsWithTag(npcTag).Length;
    }

    private bool ValidateSetup()
    {
        if (npcPrefab == null)
        {
            Debug.LogError($"{name}: NPC Prefab is not assigned.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(npcTag))
        {
            Debug.LogError($"{name}: NPC Tag is not assigned.");
            return false;
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError($"{name}: No waypoints are assigned.");
            return false;
        }

        return true;
    }
}