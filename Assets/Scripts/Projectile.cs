using PurrNet.Prediction;
using UnityEngine;

public class Projectile : PredictedIdentity<Projectile.State>
{
    [SerializeField] private PredictedRigidbody rigidBody;
    [SerializeField] private float knockForce = 5f;
    [SerializeField] private ParticleSystem explosion;

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

        Instantiate(explosion, transform.position, Quaternion.identity);
        predictionManager.hierarchy.Delete(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement otherPlayer))
            return;

        Instantiate(explosion, transform.position, Quaternion.identity);
        predictionManager.hierarchy.Delete(gameObject);
    }

    public struct State : IPredictedData<State>
    {
        public void Dispose()
        {

        }
    }
}