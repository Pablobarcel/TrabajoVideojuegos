using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerWallJump))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement3D : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerJump jumpScript;
    private PlayerWallJump wallJumpScript;
    private PlayerStats stats;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        jumpScript = GetComponent<PlayerJump>();
        wallJumpScript = GetComponent<PlayerWallJump>();
        stats = GetComponent<PlayerStats>();

        if (stats == null)
        {
            Debug.LogError("PlayerStats no encontrado en el jugador.");
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = stats.ActiveStats.moveSpeed;
        float wallSlideSpeed = stats.ActiveStats.wallSlideSpeed;

        float move = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move * moveSpeed;
        velocity.z = 0f;

        // Si está deslizando por la pared, limitar la caída
        if (wallJumpScript.IsWallSliding && velocity.y < -wallSlideSpeed)
        {
            velocity.y = -wallSlideSpeed;
            Debug.Log($"Velocidad de deslizamiento en pared: {velocity.y}");
        }

        rb.linearVelocity = velocity;
    }
}
