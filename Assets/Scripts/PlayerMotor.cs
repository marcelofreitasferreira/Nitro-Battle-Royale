using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -18f;
    private CharacterController controller;
    private float verticalVelocity;

    private void Awake() { controller = GetComponent<CharacterController>(); }

    public override void OnNetworkSpawn()
    {
        controller.enabled = IsOwner;
        var camera = Camera.main;
        if (camera != null && IsOwner)
        {
            var follow = camera.GetComponent<SimpleFollowCamera>();
            if (follow != null) follow.target = transform;
        }
    }

    private void Update()
    {
        if (!IsOwner || !controller.enabled) return;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        if (direction.sqrMagnitude > 0.01f) transform.forward = direction.normalized;
        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -1f;
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move((direction * moveSpeed + Vector3.up * verticalVelocity) * Time.deltaTime);
    }
}
