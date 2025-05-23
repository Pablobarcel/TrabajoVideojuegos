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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpScript = GetComponent<PlayerJump>();
        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
            Debug.LogError("PlayerStats component not found.");
    }

    void Update()
    {
        CheckForWall();

        float wallSlideSpeed = playerStats != null ? playerStats.ActiveStats.wallSlideSpeed : 1f;

        if (IsWallSliding && rb.linearVelocity.y < -wallSlideSpeed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallSlideSpeed, rb.linearVelocity.z);
        }
    }

    void CheckForWall()
    {
        IsWallSliding = false;
        lastWallNormal = Vector3.zero;

        // Detecta todas las colisiones cercanas
        Collider[] hits = Physics.OverlapSphere(transform.position, wallCheckRadius, wallLayer);

        foreach (var hit in hits)
        {
            if (!jumpScript.IsGrounded && rb.linearVelocity.y < 0)
            {
                // Calculamos la normal más cercana
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

        // Invertimos la dirección horizontal del salto con base en la pared
        Vector3 jumpDir = Vector3.Reflect(wallJumpDirection, lastWallNormal).normalized;
        float wallJumpForce = playerStats.ActiveStats.wallJumpForce;

        rb.linearVelocity = jumpDir * wallJumpForce;
        IsWallSliding = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wallCheckRadius);
    }
}
