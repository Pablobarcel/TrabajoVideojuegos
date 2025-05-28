using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PhantomAI : MonoBehaviour
{
    public float patrolHeight = 5f;
    public float patrolSpeed = 3f;
    public float detectionRange = 8f;
    public float attackCooldown = 4f;

    public Transform groundCheck; // Asignar en el Inspector
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;


    private Transform player;
    private Rigidbody rb;
    private EnemyStats stats;
    private Vector3 initialPosition;
    private float initialY;
    private bool isStunned = false;
    private bool isAttacking = false;
    private float lastAttackTime = -Mathf.Infinity;
    
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        initialPosition = transform.position;
        initialY = initialPosition.y; // Guardamos la altura fija
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || isStunned || isAttacking) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (Time.time - lastAttackTime >= attackCooldown && distance <= detectionRange)
        {
            Debug.Log("Phantom decide atacar con Ataque Potente.");
            StartCoroutine(PowerfulDiveAttack());

            lastAttackTime = Time.time;
        }
        else
        {
            PatrolAbovePlayer();
        }
    }

    void PatrolAbovePlayer()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(player.position.x, initialY, transform.position.z);
        Vector3 direction = (targetPos - transform.position).normalized;
        rb.linearVelocity = new Vector3(direction.x * patrolSpeed, 0f, 0f); // SOLO en X
    }


    IEnumerator TripleStrikeAttack()
    {
        isAttacking = true;

        for (int i = 0; i < 3; i++)
        {
            // Capturar la posición actual del jugador UNA VEZ por ataque
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
            yield return MoveToHeight(initialY, 15f);
            Debug.Log($"Phantom golpea al jugador ({i + 1}/3)");

            Collider[] hits = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Player"));
            foreach (Collider col in hits)
            {
                PlayerHealth ph = col.GetComponent<PlayerHealth>();
                if (ph && !ph.IsInvisible())
                    ph.TakeDamage(stats.damage, transform.position);
            }

            yield return new WaitForSeconds(0.3f);

            Vector3 recoveryPos = new Vector3(transform.position.x, initialY, transform.position.z);
            yield return MoveToHeight(initialY, 10f);
            yield return new WaitForSeconds(0.2f);
        }

        isAttacking = false;
    }

    IEnumerator PowerfulDiveAttack()
    {
        isAttacking = true;
        StartCoroutine(TriggerAnimatorBool("Attack", 0.6f));
        Debug.Log("Phantom desciende en picado.");

        // Desactivar movimiento horizontal
        rb.linearVelocity = Vector3.zero;

        // Bajar en vertical hasta detectar suelo o jugador
        while (true)
        {
            Vector3 directionToPlayer = (new Vector3(player.position.x, player.position.y, transform.position.z) - transform.position).normalized;
            yield return new WaitForSeconds(0.2f);
            rb.linearVelocity = directionToPlayer * 25f;


            bool hitGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

            if (hitGround)
            {
                Debug.Log("Phantom golpea el suelo");
                break;
            }

            yield return null;
        }

        // Esperar un poco tras el impacto
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);

        // Subir de nuevo a la altura inicial
        Debug.Log("Phantom regresa a su altura.");
        yield return MoveToHeight(initialY, 15f);

        isAttacking = false;
    }


    IEnumerator MoveToHeight(float targetY, float speed)
    {
        while (Mathf.Abs(transform.position.y - targetY) > 1.5f)
        {
            Debug.Log($"Phantom moviéndose a altura {targetY}. Posición actual: {transform.position.y}");
            float direction = targetY > transform.position.y ? 1f : -1f;
            rb.linearVelocity = new Vector3(0f, direction * speed, 0f);
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
    }




    IEnumerator Stun(float seconds)
    {
        StartCoroutine(TriggerAnimatorBool("IsHurt", 0.6f));
        isStunned = true;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(seconds);
        isStunned = false;
    }

    public IEnumerator StunAndKnockback(Vector3 sourcePosition)
    {
        yield return Stun(0.5f);

        Vector3 knockbackDir = (transform.position - sourcePosition).normalized;
        knockbackDir.y = 0f;
        rb.AddForce(knockbackDir * 4f, ForceMode.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && !health.IsInvisible())
            {
                health.TakeDamage(stats.damage, transform.position);
            }
        }
    }

    private IEnumerator TriggerAnimatorBool(string parameter, float duration)
    {
        animator.SetBool(parameter, true);
        yield return new WaitForSeconds(duration);
        animator.SetBool(parameter, false);
    }


}
