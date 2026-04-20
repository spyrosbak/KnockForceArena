using UnityEngine;
using PurrNet.Prediction;

public class Shoot : PredictedIdentity<Shoot.Input, Shoot.State>
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float shootForce = 20.0f;

    protected override void Simulate(Input input, ref State state, float delta)
    {
        if (state.canShoot && input.shoot)
        {
            state.shootCooldown = 1.0f;

            var realDirection = new Vector3(input.direction.x, input.direction.y, input.direction.z);
            var spawnPoint = gunPoint.position + realDirection;
            var createdObject = predictionManager.hierarchy.Create(projectile.gameObject, spawnPoint, Quaternion.identity);

            if (!createdObject.HasValue)
                return;

            if (!createdObject.Value.TryGetComponent(predictionManager, out PredictedRigidbody rb))
                return;

            rb.AddForce(realDirection * shootForce, ForceMode.Impulse);
        }
        else
        {
            state.shootCooldown -= delta;
        }
    }

    protected override void UpdateInput(ref Input input)
    {
        input.shoot |= InputManager.Instance.fireAction.triggered;
    }

    protected override void ModifyExtrapolatedInput(ref Input input)
    {
        input.shoot = false;
    }

    protected override void GetFinalInput(ref Input input)
    {
        
        input.direction = Camera.main.transform.forward;
    }

    protected override void SanitizeInput(ref Input input)
    {
        input.direction.Normalize();
    }

    public struct Input : IPredictedData
    {
        public bool shoot;
        public Vector3 direction;

        public void Dispose()
        {

        }
    }

    public struct State : IPredictedData<State>
    {
        public float shootCooldown;
        public bool canShoot => shootCooldown <= 0;

        public void Dispose()
        {

        }
    }
}