using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Ground Check")]
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [HideInInspector] public bool IsGrounded { get; private set; }

    private int jumpsRemaining;
    private Rigidbody rb;
    private PlayerWallJump wallJumpScript;
    private PlayerStats stats;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wallJumpScript = GetComponent<PlayerWallJump>();
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();

        if (stats == null)
        {
            Debug.LogError("PlayerStats no encontrado en el jugador.");
        }
    }

    void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        animator.SetBool("IsGrounded", IsGrounded);

        // Actualizar JumpVelocity para animación
        float verticalVelocity = rb.linearVelocity.y;
        animator.SetFloat("JumpVelocity", verticalVelocity);

        if (IsGrounded)
        {
            jumpsRemaining = stats.ActiveStats.maxJumps;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallJumpScript.IsWallSliding)
            {
                wallJumpScript.WallJump();
                jumpsRemaining--;
            }
            else if (jumpsRemaining > 0)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        float jumpForce = stats.ActiveStats.jumpForce;

        Vector3 jumpVelocity = rb.linearVelocity;
        jumpVelocity.y = jumpForce;
        rb.linearVelocity = jumpVelocity;

        jumpsRemaining--;

        Debug.Log($"Saltando con fuerza: {jumpForce}");
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
