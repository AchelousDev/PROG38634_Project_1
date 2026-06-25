using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

    public float Velocity;
    [Space]

	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    public float verticalVel;
    private Vector3 moveVector;

    private float mobileInputX;
    private float mobileInputZ;

    private float joystickInputX;
    private float joystickInputZ;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		InputMagnitude ();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);


    }

    void PlayerMoveAndRotation() {
		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false && desiredMoveDirection.sqrMagnitude > 0.01f) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
		}
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

	void InputMagnitude() {
		//Calculate Input Vectors from keyboard and mobile UI buttons
		InputX = Mathf.Clamp(Input.GetAxis ("Horizontal") + mobileInputX + joystickInputX, -1f, 1f);
		InputZ = Mathf.Clamp(Input.GetAxis ("Vertical") + mobileInputZ + joystickInputZ, -1f, 1f);

		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		Speed = Mathf.Clamp01(new Vector2(InputX, InputZ).sqrMagnitude);

        //Physically move player

		if (Speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}

    public void MobileForwardDown()
    {
        mobileInputZ = 1f;
    }

    public void MobileBackwardDown()
    {
        mobileInputZ = -1f;
    }

    public void MobileLeftDown()
    {
        mobileInputX = -1f;
    }

    public void MobileRightDown()
    {
        mobileInputX = 1f;
    }

    public void MobileVerticalUp()
    {
        mobileInputZ = 0f;
    }

    public void MobileHorizontalUp()
    {
        mobileInputX = 0f;
    }

    public void SetJoystickInput(Vector2 direction)
    {
        joystickInputX = Mathf.Clamp(direction.x, -1f, 1f);
        joystickInputZ = Mathf.Clamp(direction.y, -1f, 1f);
    }

    public void SetJoystickInput(float horizontal, float vertical)
    {
        joystickInputX = Mathf.Clamp(horizontal, -1f, 1f);
        joystickInputZ = Mathf.Clamp(vertical, -1f, 1f);
    }

    public void ResetJoystickInput()
    {
        joystickInputX = 0f;
        joystickInputZ = 0f;
    }
}
