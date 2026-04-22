using PurrNet.Prediction;
using UnityEngine;

public class PlayerMovement : PredictedIdentity<PlayerMovement.PlayerInput, PlayerMovement.PlayerState>
{
    [SerializeField] private PredictedRigidbody rigidBody;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private GameObject gun;
    private bool grounded;

    [Header("Camera")]
    [SerializeField] private PlayerCamera cam;

    protected override void LateAwake()
    {
        base.LateAwake();

        if (isOwner)
            cam.Init();
    }

    protected override void Simulate(PlayerInput input, ref PlayerState state, float delta)
    {
        if (input.cameraForward.HasValue)
        {
            var forwardDir = input.cameraForward.Value;
            var gunDir = input.cameraForward.Value;
            forwardDir.y = 0.0f;
            gunDir.z = 0.0f;

            if (forwardDir.sqrMagnitude > 0.0001f)
            {
                rigidBody.MoveRotation(Quaternion.LookRotation(forwardDir.normalized));
                gun.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
            }
                
        }

        if (input.jump && grounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            grounded = false;
        }
    }

    protected override void LateSimulate(PlayerInput input, ref PlayerState state, float delta)
    {
        Vector3 moveDirection = new Vector3(input.direction.x, 0, input.direction.y).normalized * speed;
        //rigidBody.AddForce(moveDirection);
        transform.Translate(moveDirection * delta);
    }

    protected override void GetFinalInput(ref PlayerInput input)
    {
        //input.direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.direction = new Vector2(InputManager.Instance.moveAction.ReadValue<Vector2>().x, InputManager.Instance.moveAction.ReadValue<Vector2>().y);
        input.cameraForward = cam.forward;
    }

    protected override void SanitizeInput(ref PlayerInput input)
    {
        if(input.direction.magnitude > 1)
            input.direction.Normalize();
    }

    protected override void UpdateInput(ref PlayerInput input)
    {
        input.jump |= InputManager.Instance.jumpAction.triggered; // |= -> || ( input.jump = Input.GetKeyDown(KeyCode.Space) || input.jump); )
    }

    protected override void ModifyExtrapolatedInput(ref PlayerInput input)
    {
        input.jump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }
    

    public struct PlayerState : IPredictedData<PlayerState>
    {
        public void Dispose()
        {

        }
    }

    public struct PlayerInput : IPredictedData
    {
        public Vector2 direction;
        public Vector3? cameraForward;
        public bool jump;

        public void Dispose()
        {

        }
    }
}