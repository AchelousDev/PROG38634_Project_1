using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 180f;
    public bool lockCursorWhileRotating = false;

    private float yaw;

    private void Start()
    {
        if (player != null)
        {
            transform.position = player.position;
            yaw = player.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }
    }
    
    public void SetYaw(float newYaw)
    {
        yaw = newYaw;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        transform.position = player.position;

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            yaw += mouseX * mouseSensitivity;

            transform.rotation = Quaternion.Euler(0f, yaw, 0f);

            if (lockCursorWhileRotating)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            if (lockCursorWhileRotating)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}