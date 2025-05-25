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

    private Vector3 moveDirection = Vector3.right;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        shootTimer = shootCooldown;
    }

    void Update()
    {
        if (rb == null || stats == null) return;

        if (PlayerInRange())
        {
            rb.linearVelocity = Vector3.zero; // Detener movimiento

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootCooldown;
            }
        }
        else
        {
            Patrol(); // Solo patrulla si el jugador NO está cerca
        }
    }



    void Patrol()
    {
        rb.linearVelocity = moveDirection * stats.patrolSpeed;

        // Cambiar dirección si toca pared (raycast hacia adelante)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, 0.5f))
        {
            if (!hit.collider.isTrigger)
            {
                moveDirection = -moveDirection;
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
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
        bullet.GetComponent<Rigidbody>().linearVelocity = direction * 10f; // Velocidad bala
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
