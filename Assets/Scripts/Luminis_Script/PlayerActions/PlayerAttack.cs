using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode hardAttackKey = KeyCode.Mouse1;
    public KeyCode specialAttackKey = KeyCode.R;
    public LayerMask enemyLayer;
    public Transform attackOrigin;

    public float hardAttackHoldTime = 1.5f;
    public float weakAttackAnimTime = 0.5f;
    public float heavyAttackAnimTime = 0.8f;

    private float hardAttackTimer = 0f;
    private bool isHoldingHardAttack = false;
    private bool isFacingRight = true;
    private bool isPerformingSpecialAttack = false;

    private PlayerStats stats;
    private PlayerJump jumpScript;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        jumpScript = GetComponent<PlayerJump>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (!stats) Debug.LogError("PlayerStats no encontrado.");
        if (!jumpScript) Debug.LogError("PlayerJump no encontrado.");
    }

    void Update()
    {
        // Dirección del jugador
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            isFacingRight = false;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            isFacingRight = true;

        // Ataque normal
        if (Input.GetKeyDown(attackKey))
            StartCoroutine(Attack());

        // Heavy Attack
        if (Input.GetKeyDown(hardAttackKey))
        {
            isHoldingHardAttack = true;
            hardAttackTimer = 0f;
            animator.SetBool("Charging", true);
        }

        if (isHoldingHardAttack)
        {
            hardAttackTimer += Time.deltaTime;

            if (Input.GetKeyUp(hardAttackKey))
            {
                isHoldingHardAttack = false;
                animator.SetBool("Charging", false);

                if (hardAttackTimer >= hardAttackHoldTime)
                    StartCoroutine(HardAttack());
            }
        }

        // Ataque especial
        if (Input.GetKeyDown(specialAttackKey) && !jumpScript.IsGrounded)
        {
            int furyCost = stats.ActiveStats.SpecialAttackFuryCost;

            if (stats.GetFuriaActual() >= furyCost)
            {
                stats.ConsumeFuria(furyCost);
                isPerformingSpecialAttack = true;
                animator.SetBool("Fury", true);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, stats.ActiveStats.SpecialAttackFallSpeed, 0f);
                Debug.Log($"Ataque especial iniciado. Furia restante: {stats.GetFuriaActual()}");
            }
            else
            {
                Debug.Log("No tienes suficiente furia para usar el ataque especial.");
            }
        }
    }

    void FixedUpdate()
    {
        if (isPerformingSpecialAttack && jumpScript.IsGrounded)
        {
            Debug.Log("Impacto del ataque especial contra el suelo");
            StartCoroutine(PerformSpecialAttack());
            isPerformingSpecialAttack = false;
        }
    }

    IEnumerator Attack()
    {
        animator.SetBool("WeakAttack", true);

        float attackRange = stats.ActiveStats.attackRange;
        int attackDamage = stats.ActiveStats.attackDamage;

        Collider[] hitEnemies = Physics.OverlapSphere(attackOrigin.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyCombatHandler combat = enemy.GetComponentInChildren<EnemyCombatHandler>();
            if (combat != null)
                combat.TakeDamage(attackDamage, gameObject);
        }

        yield return new WaitForSeconds(weakAttackAnimTime);
        animator.SetBool("WeakAttack", false);
    }

    IEnumerator HardAttack()
    {
        animator.SetBool("HeavyAttack", true);

        float range = stats.ActiveStats.HardAttackRange;
        int damage = stats.ActiveStats.HardAttackDamage;

        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, range, enemyLayer);
        foreach (Collider enemy in hits)
        {
            bool enemyIsRight = enemy.transform.position.x > transform.position.x;
            if (isFacingRight == enemyIsRight)
            {
                EnemyCombatHandler combat = enemy.GetComponentInChildren<EnemyCombatHandler>();
                if (combat != null)
                {
                    combat.TakeDamage(damage, gameObject);

                    Rigidbody enemyRb = enemy.GetComponentInParent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                        Vector3 force = new Vector3(knockbackDir.x, 0.1f, 0f) * 250f;
                        enemyRb.AddForce(force, ForceMode.Impulse);
                    }
                }
            }
        }

        yield return new WaitForSeconds(heavyAttackAnimTime);
        animator.SetBool("HeavyAttack", false);
    }

    IEnumerator PerformSpecialAttack()
    {
        float range = stats.ActiveStats.SpecialAttackRange;
        int damage = stats.ActiveStats.SpecialAttackDamage;

        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        Debug.Log($"Especial golpea a {enemies.Length} enemigos.");

        bool hitEnemy = false;
        foreach (Collider enemy in enemies)
        {
            EnemyCombatHandler combat = enemy.GetComponentInChildren<EnemyCombatHandler>();
            if (combat != null)
            {
                combat.TakeDamage(damage, gameObject);
                hitEnemy = true;
            }
        }

        if (hitEnemy)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 10f, rb.linearVelocity.z);
        }

        yield return new WaitForSeconds(0.5f); // Duración estimada del "impacto" especial
        animator.SetBool("Fury", false);
    }

    public bool IsPerformingSpecialAttack => isPerformingSpecialAttack;

    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, 1f);

#if UNITY_EDITOR
        if (Application.isPlaying && stats != null)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, stats.ActiveStats.SpecialAttackRange);
        }
#endif
    }
}