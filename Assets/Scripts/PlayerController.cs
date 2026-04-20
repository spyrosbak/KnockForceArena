using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private Rigidbody rb;
    private Vector2 move;
    private Vector2 look;

    [Header("Camera")]
    [SerializeField] private Transform cameraTarget;
    private float yaw;
    private float pitch;
    private float clampTop = 70.0f;
    private float clampBottom = -30.0f;
    private float lookSpeed = 10.0f;

    [Header("Movement")]
    private float movementSpeed = 3.0f;
    [SerializeField] private TrailRenderer trai;
    [SerializeField] private NetworkAnimator anim;

    private void Awake()
    {
        playerInput = inputActions.FindActionMap("Player");

        moveAction = playerInput.FindAction("Move");
        lookAction = playerInput.FindAction("Look");
        jumpAction = playerInput.FindAction("Jump");

        move = moveAction.ReadValue<Vector2>();
        look = lookAction.ReadValue<Vector2>();

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Move()
    {
        float speed = moveAction.ReadValue<Vector2>().magnitude * movementSpeed;

        Vector3 forward = cameraTarget.forward;
        Vector3 right = cameraTarget.right;

        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveAction.ReadValue<Vector2>().y + right * moveAction.ReadValue<Vector2>().x).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10.0f);

            Vector3 currentVelocity = rb.linearVelocity;
            rb.linearVelocity = new Vector3(moveDirection.x * speed, currentVelocity.y, moveDirection.z * speed);

            anim.SetBool("IsWalking", true);
            trai.emitting = true;
        }
        else
        {
            Vector3 currentVelocity = rb.linearVelocity;
            rb.linearVelocity = new Vector3(0, currentVelocity.y, 0);

            anim.SetBool("IsWalking", false);
            trai.emitting = false;
        }
    }

    private void Look()
    {
        if(lookAction.ReadValue<Vector2>().sqrMagnitude > 0.01f)
        {
            float rotationMultiplier = lookSpeed * Time.deltaTime;
            yaw -= lookAction.ReadValue<Vector2>().y * rotationMultiplier;
            pitch += lookAction.ReadValue<Vector2>().x * rotationMultiplier;
        }
        
        yaw = ClampAngle(yaw, clampBottom, clampTop);
        pitch = ClampAngle(pitch, float.MinValue, float.MaxValue);

        cameraTarget.rotation = Quaternion.Euler(yaw, pitch, 0f);
    }

    private void Jump()
    {

    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;

        if (angle > 360f)
            angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }
}