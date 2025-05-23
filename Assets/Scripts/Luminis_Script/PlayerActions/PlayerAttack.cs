using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode hardAttackKey = KeyCode.Mouse1;
    public KeyCode specialAttackKey = KeyCode.R;
    public LayerMask enemyLayer;
    public Transform attackOrigin;

    public float hardAttackHoldTime = 1.5f;

    private float hardAttackTimer = 0f;
    private bool isHoldingHardAttack = false;
    private bool isFacingRight = true;
    private bool isPerformingSpecialAttack = false;

    private PlayerStats stats;
    private PlayerJump jumpScript;
    private Rigidbody rb;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        jumpScript = GetComponent<PlayerJump>();
        rb = GetComponent<Rigidbody>();

        if (stats == null)
            Debug.LogError("PlayerStats no encontrado.");

        if (jumpScript == null)
            Debug.LogError("PlayerJump no encontrado.");
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
            Attack();

        // Hard Attack
        if (Input.GetKeyDown(hardAttackKey))
        {
            isHoldingHardAttack = true;
            hardAttackTimer = 0f;
        }

        if (isHoldingHardAttack)
        {
            hardAttackTimer += Time.deltaTime;

            if (Input.GetKeyUp(hardAttackKey))
            {
                isHoldingHardAttack = false;

                if (hardAttackTimer >= hardAttackHoldTime)
                    HardAttack();
            }
        }

        if (Input.GetKeyDown(specialAttackKey) && !jumpScript.IsGrounded)
        {
            int furyCost = stats.ActiveStats.SpecialAttackFuryCost;

            if (stats.GetFuriaActual() >= furyCost)
            {
                stats.ConsumeFuria(furyCost);
                isPerformingSpecialAttack = true;
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
            PerformSpecialAttack();
            isPerformingSpecialAttack = false;
        }
    }

    void Attack()
    {
        float attackRange = stats.ActiveStats.attackRange;
        int attackDamage = stats.ActiveStats.attackDamage;

        Collider[] hitEnemies = Physics.OverlapSphere(attackOrigin.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyCombatHandler combat = enemy.GetComponentInChildren<EnemyCombatHandler>();
            if (combat != null)
                combat.TakeDamage(attackDamage, gameObject);
        }
    }

    void HardAttack()
    {
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

                    // Aplicar retroceso
                    Rigidbody enemyRb = enemy.GetComponentInParent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                        Vector3 force = new Vector3(knockbackDir.x, 0.1f, 0f) * 250f; // Ajusta la magnitud
                        enemyRb.AddForce(force, ForceMode.Impulse);
                    }
                }
            }
        }
    }


    void PerformSpecialAttack()
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

        // Rebote si golpeó al menos un enemigo
        if (hitEnemy)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 10f, rb.linearVelocity.z); // valor ajustable
        }
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
