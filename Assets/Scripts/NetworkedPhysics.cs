using UnityEngine;
using PurrNet;
using System;

public class NetworkedPhysics : NetworkBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float bounceForce = 10.0f;
    private bool canJump;

    private struct InputData
    {
        public Vector2 velocity;
        public bool jump;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);

        if (asServer)
            return;

        enabled = isOwner;
        rb.isKinematic = !isServer;

        if (isOwner)
        {
            networkManager.onTick += OnTick;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        networkManager.onTick -= OnTick;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            canJump = true;
    }

    private void OnTick(bool asServer)
    {
        if (asServer)
            return;

        var input = new InputData()
        {
            velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            jump = canJump
        };

        canJump = false;

        Move(input);
    }

    [ServerRpc]
    private void Move(InputData inputData)
    {
        var movement = new Vector3(inputData.velocity.x, 0, inputData.velocity.y) * moveSpeed;

        rb.AddForce(movement);

        if (inputData.jump)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
            return;

        if(collision.gameObject.TryGetComponent(out NetworkedPhysics otherPlayer))
        {
            var direction = (transform.position - otherPlayer.transform.position).normalized;
            rb.AddForce(direction * bounceForce, ForceMode.Impulse);
        }
    }
}