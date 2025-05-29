using UnityEngine;
using System.Collections;

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

    private Animator animator;
    private Camera mainCamera;

    private Vector3 originalScale;

    

    private void Start()
{
    rb = GetComponent<Rigidbody>();
    stats = GetComponent<EnemyStats>();
    animator = GetComponent<Animator>();
    mainCamera = Camera.main;

    shootTimer = shootCooldown;
    directionChangeTimer = directionChangeInterval;

    originalScale = transform.localScale; // ← Guardamos la escala original
    SetRandomDirection();
}

    private void Update()
    {
        if (rb == null || stats == null) return;

        // Always face the camera
        FaceCamera();

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

    private void Patrol()
{
    directionChangeTimer -= Time.deltaTime;

    if (directionChangeTimer <= 0f)
    {
        SetRandomDirection();
        directionChangeTimer = directionChangeInterval;
    }

    rb.linearVelocity = moveDirection * stats.patrolSpeed;

    // Flip sprite horizontalmente sin alterar la escala original
    if (moveDirection.x < 0)
        transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    else if (moveDirection.x > 0)
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
}
    private void SetRandomDirection()
    {
        Vector3 newDirection;
        do
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-0.5f, 0.5f);
            newDirection = new Vector3(x, y, 0f).normalized;
        } while (newDirection == Vector3.zero);

        moveDirection = newDirection;
    }

    private bool PlayerInRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= detectionRange;
    }

    private void Shoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        bullet.GetComponent<Rigidbody>().linearVelocity = direction * 10f;

        StartCoroutine(TriggerAnimation("Attack", 1f));
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

            StartCoroutine(TriggerAnimation("IsHurt", 0.5f));
        }
        else
        {
            SetRandomDirection(); // Change direction on collision
        }
    }

    private IEnumerator TriggerAnimation(string param, float duration)
    {
        if (animator != null)
        {
            animator.SetBool(param, true);
            yield return new WaitForSeconds(duration);
            animator.SetBool(param, false);
        }
    }

    private void FaceCamera()
    {
        if (mainCamera != null)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            transform.forward = new Vector3(cameraForward.x, cameraForward.y, cameraForward.z);
        }
    }
}
