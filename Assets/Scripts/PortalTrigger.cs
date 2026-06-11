using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportExit;
    public Transform lookAtTarget;

    [Header("Optional")]
    public bool teleportOnlyOnce = true;

    private bool hasTeleported = false;

    private void OnTriggerEnter(Collider other)
    {
        if (teleportOnlyOnce && hasTeleported)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (teleportExit == null)
        {
            Debug.LogWarning("Teleport Exit is not assigned.");
            return;
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayTeleportSound();
        }

        MovementInput movement = other.GetComponent<MovementInput>();
        CharacterController controller = other.GetComponent<CharacterController>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        other.transform.position = teleportExit.position;

        if (lookAtTarget != null)
        {
            Vector3 lookDirection = lookAtTarget.position - teleportExit.position;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                other.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        else
        {
            other.transform.rotation = teleportExit.rotation;
        }

        CameraTargetController cameraTarget = FindFirstObjectByType<CameraTargetController>();

        if (cameraTarget != null)
        {
            cameraTarget.SetYaw(other.transform.eulerAngles.y);
        }

        if (controller != null)
        {
            controller.enabled = true;
        }

        if (movement != null)
        {
            movement.enabled = true;
        }

        hasTeleported = true;
    }
}