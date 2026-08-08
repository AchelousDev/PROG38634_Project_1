using PinePie.SimpleJoystick;
using UnityEngine;

public class PinePieJoystickMovementBridge : MonoBehaviour
{
    [Header("Movement")]
    public MovementInput playerMovement;
    public JoystickController joystickController;

    [Header("Spell")]
    public WizardSpellController spellController;
    public float tapThreshold = 40f;

    [Header("Camera Swipe Rotation")]
    public Transform cameraTarget;
    public float cameraSensitivity = 0.25f;
    public float minPitch = -25f;
    public float maxPitch = 60f;

    private float cameraYaw;
    private float cameraPitch;
    private Vector2 previousMousePosition;

    private Vector2 touchStartPosition;
    private int spellTouchFingerId = -1;

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
        UpdateCameraAndSpellTouch();
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

    private void UpdateCameraAndSpellTouch()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // Only use the right half of the screen
                if (touch.position.x < Screen.width * 0.5f)
                {
                    continue;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPosition = touch.position;
                    spellTouchFingerId = touch.fingerId;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (cameraTarget != null)
                    {
                        RotateCamera(touch.deltaPosition);
                    }
                }

                if (touch.phase == TouchPhase.Ended &&
                    touch.fingerId == spellTouchFingerId)
                {
                    float touchDistance =
                        Vector2.Distance(touchStartPosition, touch.position);

                    if (touchDistance <= tapThreshold)
                    {
                        CastSpell();
                    }

                    spellTouchFingerId = -1;
                }

                if (touch.phase == TouchPhase.Canceled &&
                    touch.fingerId == spellTouchFingerId)
                {
                    spellTouchFingerId = -1;
                }
            }
        }

#if UNITY_EDITOR
        UpdateMouseTesting();
#endif
    }

#if UNITY_EDITOR
    private void UpdateMouseTesting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
            touchStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) &&
            Input.mousePosition.x >= Screen.width * 0.5f)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 mouseDelta =
                currentMousePosition - previousMousePosition;

            previousMousePosition = currentMousePosition;

            if (cameraTarget != null)
            {
                RotateCamera(mouseDelta);
            }
        }

        if (Input.GetMouseButtonUp(0) &&
            Input.mousePosition.x >= Screen.width * 0.5f)
        {
            float mouseDistance = Vector2.Distance(
                touchStartPosition,
                (Vector2)Input.mousePosition
            );

            if (mouseDistance <= tapThreshold)
            {
                CastSpell();
            }
        }
    }
#endif

    private void CastSpell()
    {
        if (spellController != null)
        {
            spellController.CastSpellFromTouch();
        }
    }

    private void RotateCamera(Vector2 delta)
    {
        cameraYaw += delta.x * cameraSensitivity;
        cameraPitch -= delta.y * cameraSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        cameraTarget.rotation =
            Quaternion.Euler(cameraPitch, cameraYaw, 0f);
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