using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyingShooterEnemy : MonoBehaviour
{
    private Rigidbody rb;
    private EnemyStats stats;

    public float detectionRange = 10f;
    public float shootCooldown = 2f;
    private float shootTimer;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private Vector3 moveDirection;
    private float directionChangeInterval = 3f;
    private float directionChangeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        shootTimer = shootCooldown;
        directionChangeTimer = directionChangeInterval;

        SetRandomDirection();
    }

    void Update()
    {
        if (rb == null || stats == null) return;

        if (PlayerInRange())
        {
            rb.linearVelocity = Vector3.zero;

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootCooldown;
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        directionChangeTimer -= Time.deltaTime;

        if (directionChangeTimer <= 0f)
        {
            SetRandomDirection();
            directionChangeTimer = directionChangeInterval;
        }

        rb.linearVelocity = moveDirection * stats.patrolSpeed;
    }

    void SetRandomDirection()
    {
        Vector3 newDirection;
        do
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-0.5f, 0.5f);
            newDirection = new Vector3(x, y, 0f).normalized;
        } while (newDirection == Vector3.zero); // Evita vector nulo

        moveDirection = newDirection;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    bool PlayerInRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= detectionRange;
    }

    void Shoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        bullet.GetComponent<Rigidbody>().linearVelocity = direction * 10f;
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
        else
        {
            SetRandomDirection(); // Cambia dirección en cualquier otra colisión
        }
    }
}
