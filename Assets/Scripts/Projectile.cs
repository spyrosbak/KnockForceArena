using PurrNet.Prediction;
using UnityEngine;

public class Projectile : PredictedIdentity<Projectile.State>
{
    [SerializeField] private PredictedRigidbody rigidBody;
    [SerializeField] private float knockForce = 5f;

    private void OnEnable()
    {
        rigidBody.onCollisionEnter += OnCollision;
    }

    private void OnDisable()
    {
        rigidBody.onCollisionExit -= OnCollision;
    }

    private void OnCollision(GameObject other, PhysicsCollision physicsEvent)
    {
        if (!other.TryGetComponent(out PlayerMovement otherPlayer))
            return;

        var knockDirection = (other.transform.position - transform.position).normalized;
        var force = rigidBody.velocity.magnitude * knockForce;
        otherPlayer.GetComponent<Rigidbody>().AddForce(knockDirection * force, ForceMode.Impulse);

        predictionManager.hierarchy.Delete(gameObject);
    }

    public struct State : IPredictedData<State>
    {
        public void Dispose()
        {

        }
    }
}