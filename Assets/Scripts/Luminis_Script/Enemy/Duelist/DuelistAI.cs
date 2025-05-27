using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DuelistAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 6f;

    private Rigidbody rb;
    private Transform player;
    private EnemyStats stats;
    private Vector3 initialScale;
    private Vector3 patrolDirection = Vector3.left;
    private bool isChasing = false;
    private bool isStunned = false;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || isStunned) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isChasing = distance <= detectionRange;
    }

    void FixedUpdate()
    {
        if (isStunned) return;

        if (isChasing)
        {
            animator.SetBool("IsMoving",true);
            Vector3 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector3(direction.x * chaseSpeed, rb.linearVelocity.y, 0f);

            // Girar hacia el jugador
            transform.localScale = new Vector3(
                Mathf.Abs(initialScale.x) * (direction.x > 0 ? 1 : -1),
                initialScale.y,
                initialScale.z
            );
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("IsMoving",true);
        rb.linearVelocity = new Vector3(patrolDirection.x * patrolSpeed, rb.linearVelocity.y, 0f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, patrolDirection, out hit, 0.5f))
        {
            if (!hit.collider.isTrigger)
            {
                patrolDirection = -patrolDirection;
                transform.localScale = new Vector3(
                    Mathf.Abs(initialScale.x) * (patrolDirection.x > 0 ? 1 : -1),
                    initialScale.y,
                    initialScale.z
                );
            }
        }
    }

    public IEnumerator StunAndKnockback(Vector3 sourcePosition)
    {
        isStunned = true;
        
        animator.SetBool("Hurt",true);
        animator.SetBool("IsMoving",false);

        // Cancelar movimiento actual
        rb.linearVelocity = Vector3.zero;

        // Calcular dirección de retroceso
        Vector3 knockbackDir = (transform.position - sourcePosition).normalized;
        knockbackDir.y = 0f; // Mantener en plano horizontal

        float knockbackForce = 4f;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Hurt",false);
        animator.SetBool("IsMoving",true);
        
        isStunned = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null && !health.IsInvisible())
            {
                health.TakeDamage(stats.damage, transform.position);
            }
        }
    }
}
