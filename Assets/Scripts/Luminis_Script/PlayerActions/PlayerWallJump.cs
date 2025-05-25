
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    [Header("Wall Jump Settings")]
    public float wallCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;
    public Vector3 wallJumpDirection = new Vector3(1, 1, 0);

    [HideInInspector] public bool IsWallSliding { get; private set; }

    private Rigidbody rb;
    private PlayerJump jumpScript;
    private PlayerStats playerStats;
    private Vector3 lastWallNormal;

    private Animator animator;

    private bool isWallJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpScript = GetComponent<PlayerJump>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();

        if (playerStats == null)
            Debug.LogError("PlayerStats component not found.");
    }

    void Update()
    {
        CheckForWall();

        float wallSlideSpeed = playerStats != null ? playerStats.ActiveStats.wallSlideSpeed : 1f;

        // Deslizamiento por pared
        if (IsWallSliding && rb.linearVelocity.y < -wallSlideSpeed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallSlideSpeed, rb.linearVelocity.z);
        }

        // Actualizar animación de salto por pared
        if (isWallJumping)
        {
            if (rb.linearVelocity.y <= 2f || jumpScript.IsGrounded)
            {
                isWallJumping = false;
                animator.SetBool("WallJump", false);
            }
        }

        // Actualizar animación de deslizamiento por pared
        animator.SetBool("Wall", IsWallSliding && !jumpScript.IsGrounded);
    }

    void CheckForWall()
    {
        IsWallSliding = false;
        lastWallNormal = Vector3.zero;

        Collider[] hits = Physics.OverlapSphere(transform.position, wallCheckRadius, wallLayer);

        foreach (var hit in hits)
        {
            if (!jumpScript.IsGrounded && rb.linearVelocity.y < 0)
            {
                Vector3 directionToWall = transform.position - hit.ClosestPoint(transform.position);
                lastWallNormal = directionToWall.normalized;

                IsWallSliding = true;
                break;
            }
        }
    }

    public void WallJump()
    {
        if (!IsWallSliding || playerStats == null || lastWallNormal == Vector3.zero)
            return;

        Vector3 jumpDir = Vector3.Reflect(wallJumpDirection, lastWallNormal).normalized;
        float wallJumpForce = playerStats.ActiveStats.wallJumpForce;

        rb.linearVelocity = jumpDir * wallJumpForce;
        IsWallSliding = false;

        // Activar animación de salto por pared
        isWallJumping = true;
        animator.SetBool("WallJump", true);
        animator.SetBool("Wall", false); // Asegura que se desactive al saltar
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wallCheckRadius);
    }
}
