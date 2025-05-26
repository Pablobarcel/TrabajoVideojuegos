using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PhantomAI : MonoBehaviour
{
    public float patrolHeight = 5f;
    public float patrolSpeed = 3f;
    public float detectionRange = 8f;
    public float attackCooldown = 4f;

    private Transform player;
    private Rigidbody rb;
    private EnemyStats stats;
    private Vector3 initialPosition;
    private float initialY;
    private bool isStunned = false;
    private bool isAttacking = false;
    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        initialPosition = transform.position;
        initialY = initialPosition.y; // Guardamos la altura fija
    }

    void Update()
    {
        if (player == null || isStunned || isAttacking) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (Time.time - lastAttackTime >= attackCooldown && distance <= detectionRange)
        {
            int attackChoice = Random.Range(0, 2); // 0 = triple golpe, 1 = potente
            if (attackChoice == 0)
            {
                Debug.Log("Phantom decide atacar con Triple Golpe.");
                StartCoroutine(TripleStrikeAttack());
            }
            else
            {
                Debug.Log("Phantom decide atacar con Ataque Potente.");
                StartCoroutine(PowerfulDiveAttack());
            }

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
        rb.linearVelocity = new Vector3(direction.x * patrolSpeed, direction.y * patrolSpeed, 0f);
    }

    IEnumerator TripleStrikeAttack()
    {
        isAttacking = true;

        for (int i = 0; i < 3; i++)
        {
            // Capturar la posición actual del jugador UNA VEZ por ataque
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
            yield return MoveToPosition(targetPos, 15f);
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
            yield return MoveToPosition(recoveryPos, 10f);
            yield return new WaitForSeconds(0.2f);
        }

        isAttacking = false;
    }

    IEnumerator PowerfulDiveAttack()
    {
        isAttacking = true;

        // Capturar posición del jugador UNA VEZ al inicio
        Vector3 diveTarget = new Vector3(player.position.x, player.position.y, transform.position.z);
        yield return MoveToPosition(diveTarget, 25f);
        Debug.Log("Phantom realiza un ataque potente.");

        bool hit = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f, LayerMask.GetMask("Player"));
        foreach (Collider col in hits)
        {
            PlayerHealth ph = col.GetComponent<PlayerHealth>();
            if (ph && !ph.IsInvisible())
            {
                ph.TakeDamage(stats.damage * 2, transform.position);
                hit = true;
            }
        }

        if (!hit)
            Debug.Log("Phantom falló el ataque potente.");

        yield return Stun(3f);

        Vector3 recoveryPos = new Vector3(transform.position.x, initialY, transform.position.z);
        yield return MoveToPosition(recoveryPos, 10f);

        isAttacking = false;
    }

    IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        while (Mathf.Abs(transform.position.y - target.y) > 0.05f || Vector3.Distance(transform.position, target) > 0.1f)
        {
            Vector3 dir = (target - transform.position).normalized;
            rb.linearVelocity = dir * speed;
            yield return null;
        }
        rb.linearVelocity = Vector3.zero;
    }


    IEnumerator Stun(float seconds)
    {
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
}
