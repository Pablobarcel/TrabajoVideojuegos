using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BossAI : MonoBehaviour
{
    public List<Transform> projectileSpawnPoints;
    public GameObject projectilePrefab;
    public float detectionRange = 15f;
    public float attackCooldown = 3f;
    public float patrolSpeed = 2f;
    public float floatHeight = 6f;

    private Rigidbody rb;
    private Transform player;
    private EnemyStats stats;
    private float lastAttackTime = -999f;
    private bool fightStarted = false;
    private float originalGravity;
    private bool isAttacking = false;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        originalGravity = Physics.gravity.y;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!fightStarted && dist <= detectionRange)
        {
            StartBossFight();
        }

        if (!fightStarted) return;

        PatrolFollowPlayer();

        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(ShootAtPlayer());
            lastAttackTime = Time.time;
        }
    }

    public void StartBossFight()
    {
        if (fightStarted) return;

        fightStarted = true;
        Physics.gravity = new Vector3(0, originalGravity * 0.6f, 0);
        Debug.Log("¡Boss ha detectado al jugador y comienza la pelea!");
    }

    private void PatrolFollowPlayer()
    {
        if (player == null) return;

        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.linearVelocity = direction * patrolSpeed;
    }

    private IEnumerator ShootAtPlayer()
    {
        isAttacking = true;

         animator.SetBool("Attack", true);
         yield return new WaitForSeconds(1f);
    

        if (player == null)
        {
            isAttacking = false;
            animator.SetBool("Attack", false);
            
            yield break;
        }

        foreach (var point in projectileSpawnPoints)
        {
            if (projectilePrefab != null && point != null)
            {
                GameObject bullet = Instantiate(projectilePrefab, point.position, Quaternion.identity);

                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                if (bulletRb != null)
                {
                    bulletRb.useGravity = false;
                    Vector3 direction = (player.position - point.position).normalized;
                    float bulletSpeed = 12f;
                    bulletRb.linearVelocity = direction * bulletSpeed;
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack", false);
        isAttacking = false;

        
    }

    void OnDestroy()
    {
        Physics.gravity = new Vector3(0, originalGravity, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null && !health.IsInvisible())
            {
                Debug.Log("Boss colisiona con el jugador, aplicando daño.");
                health.TakeDamage(stats.damage, transform.position);
            }
        }
    }

     
}

