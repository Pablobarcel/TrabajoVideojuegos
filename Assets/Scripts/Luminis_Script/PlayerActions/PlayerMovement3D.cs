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

    private Animator animator;
    public static bool isFacingRight = true;

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

        animator = GetComponent<Animator>();
        isFacingRight = true;
    }

    void FixedUpdate()
    {
        float moveSpeed = stats.ActiveStats.moveSpeed;
        float wallSlideSpeed = stats.ActiveStats.wallSlideSpeed;

        float move = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move * moveSpeed;
        velocity.z = 0f;

        // Wall sliding
        if (wallJumpScript.IsWallSliding && velocity.y < -wallSlideSpeed)
        {
            velocity.y = -wallSlideSpeed;
            Debug.Log($"Velocidad de deslizamiento en pared: {velocity.y}");
        }

        rb.linearVelocity = velocity;

        // Animator: activar o desactivar "IsWalking"
        if (animator != null)
        {
            bool isWalking = Mathf.Abs(move) > 0.01f;
            animator.SetBool("IsWalking", isWalking);
        }

        // Flip del personaje
        if (move > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (move < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
