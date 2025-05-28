using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovementController : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float wallCheckDistance = 0.5f;
    public float groundCheckDistance = 1f;
    public LayerMask obstacleLayer;
    public Transform groundCheck;
    public Transform wallCheck;

    private Rigidbody rb;
    private EnemyStats stats;
    private Vector3 patrolDirection = Vector3.right;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();

        if (stats == null)
        {
            Debug.LogError("EnemyStats no encontrado en " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Chase();
        }
        else
        {
            Patrol();

            if (IsFacingWall() || IsAtEdge())
            {
                Flip();
            }
        }
    }

    void Patrol()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.x = patrolDirection.x * stats.patrolSpeed;
        velocity.z = 0f; // mantener en plano 2D
        rb.linearVelocity = velocity;
    }

    void Chase()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        direction.z = 0f;
        Vector3 velocity = new Vector3(direction.x * stats.chaseSpeed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = velocity;
    }

    bool IsFacingWall()
    {
        return Physics.Raycast(wallCheck.position, patrolDirection, wallCheckDistance, obstacleLayer);
    }

    bool IsAtEdge()
    {
        return !Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, obstacleLayer);
    }

    void Flip()
{
    patrolDirection *= -1;

    // Voltear solo el modelo visual, no el objeto completo
    Transform visual = transform.Find("Enemigo2");
    if (visual != null)
    {
        Vector3 scale = visual.localScale;
        scale.x *= -1;
        visual.localScale = scale;
    }
    else
    {
       // Debug.LogWarning("No se encontró el hijo 'Enemigo2' para hacer flip.");
    }
}


    public void SetTarget(Transform player)
    {
        target = player;
    }

    public void ClearTarget()
    {
        target = null;
        rb.linearVelocity = Vector3.zero;
    }

    void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + patrolDirection * wallCheckDistance);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
