using UnityEngine;
using PurrNet.Prediction;

public class Ball : PredictedIdentity<PlayerInput, PlayerState>
{
    [SerializeField] private PredictedRigidbody rigidBody;
    [SerializeField] private float knockbackForce = 4.0f;

    private void OnEnable()
    {
        rigidBody.onCollisionEnter += OnCollisionStart;
    }

    private void OnDisable()
    {
        rigidBody.onCollisionExit -= OnCollisionStart;
    }

    private void OnCollisionStart(GameObject other, PhysicsCollision physicsEvent)
    {
        if (!other.TryGetComponent(out PlayerMovement otherPlayer))
            return;

        var knockDirection = (transform.position - other.transform.position).normalized;
        rigidBody.AddForce(knockDirection * knockbackForce, ForceMode.Impulse);
    }

    protected override void Simulate(PlayerInput input, ref PlayerState state, float delta)
    {
        //Vector3 moveDirection = new Vector3(input.direction.x, 0, input.direction.y).normalized * speed;
        //rigidBody.AddForce(moveDirection);

        //if (input.jump && grounded)
        //{
        //    rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //    grounded = false;
        //}
    }

    //protected override void GetFinalInput(ref PlayerInput input)
    //{
    //    input.direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //}

    //protected override void UpdateInput(ref PlayerInput input)
    //{
    //    input.jump |= Input.GetKeyDown(KeyCode.Space); // |= -> || ( input.jump = Input.GetKeyDown(KeyCode.Space) || input.jump); )
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //        grounded = true;
    //}
}

public struct PlayerState : IPredictedData<PlayerState>
{
    public void Dispose()
    {
        
    }
}

public struct PlayerInput : IPredictedData
{
    //public Vector2 direction;
    //public bool jump;

    public void Dispose()
    {
        
    }
}