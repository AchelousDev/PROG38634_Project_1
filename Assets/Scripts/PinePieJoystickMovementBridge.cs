using PinePie.SimpleJoystick;
using UnityEngine;

public class PinePieJoystickMovementBridge : MonoBehaviour
{
    [Header("Movement")]
    public MovementInput playerMovement;
    public JoystickController joystickController;

    [Header("Camera Swipe Rotation")]
    public Transform cameraTarget;
    public float cameraSensitivity = 0.25f;
    public float minPitch = -25f;
    public float maxPitch = 60f;

    private float cameraYaw;
    private float cameraPitch;
    private Vector2 previousMousePosition;

    private void Start()
    {
        if (cameraTarget != null)
        {
            Vector3 startRotation = cameraTarget.eulerAngles;
            cameraYaw = startRotation.y;
            cameraPitch = NormalizeAngle(startRotation.x);
        }
    }

    private void Update()
    {
        UpdateJoystickMovement();
        UpdateCameraSwipeRotation();
    }

    private void UpdateJoystickMovement()
    {
        if (playerMovement == null || joystickController == null)
        {
            return;
        }

        Vector2 input = joystickController.InputDirection;
        playerMovement.SetJoystickInput(input);
    }

    private void UpdateCameraSwipeRotation()
    {
        if (cameraTarget == null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width * 0.5f)
                {
                    continue;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    RotateCamera(touch.deltaPosition);
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && Input.mousePosition.x >= Screen.width * 0.5f)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 mouseDelta = currentMousePosition - previousMousePosition;
            previousMousePosition = currentMousePosition;

            RotateCamera(mouseDelta);
        }
#endif
    }

    private void RotateCamera(Vector2 delta)
    {
        cameraYaw += delta.x * cameraSensitivity;
        cameraPitch -= delta.y * cameraSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}