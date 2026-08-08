using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EndpointGuardController : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRadius = 6f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Animator")]
    [SerializeField] private string saluteTrigger = "Salute";

    private Animator animator;
    private bool hasSaluted = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            FacePlayer();

            if (!hasSaluted)
            {
                animator.SetTrigger(saluteTrigger);
                hasSaluted = true;
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}