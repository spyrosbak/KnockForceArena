using UnityEngine;
using PurrNet.Prediction;

public class Shoot : PredictedIdentity<Shoot.Input, Shoot.State>
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private Transform aimEnd;
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

            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            rb.velocity = ray.direction * shootForce;
            rb.AddForce(ray.direction * shootForce, ForceMode.Impulse);
        }
        else
        {
            state.shootCooldown -= delta;
        }
    }

    protected override void UpdateInput(ref Input input)
    {
        input.shoot |= InputManager.Instance.fireAction.triggered;

        aimLine.SetPosition(0, gunPoint.position);
        aimLine.SetPosition(1, aimEnd.position);
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