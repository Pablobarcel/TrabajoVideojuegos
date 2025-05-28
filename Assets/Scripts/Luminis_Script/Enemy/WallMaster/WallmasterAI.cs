using UnityEngine;
using System.Collections;

public class WallmasterAI : MonoBehaviour
{
    public Transform[] wallPositions; // Puntos de pared
    public float jumpForce = 12f;
    public float chargeForce = 10f;
    public float decisionCooldown = 3f;

    private Transform player;
    private Rigidbody rb;
    private EnemyStats stats;
    private bool isCharging = false;
    private bool isInWallState = false;

    Animator animator; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isCharging && !isInWallState)
        {
            StartCoroutine(DecisionRoutine());
        }
    }

    IEnumerator DecisionRoutine()
    {
        // Bloquea múltiples decisiones simultáneas
        isCharging = true;

        int action = Random.Range(0, 2);

        if (action == 0 && wallPositions.Length > 0)
        {
            StartCoroutine(JumpToWallAndAttack());
        }
        else
        {
            StartCoroutine(GroundCharge());
            yield return new WaitForSeconds(2.5f); // Duración del charge
            animator.SetBool("Attack",false);
        }

        // Espera antes de permitir siguiente acción
        yield return new WaitForSeconds(decisionCooldown);
        isCharging = false;
    }

    IEnumerator JumpToWallAndAttack()
    {
        isInWallState = true;
        animator.SetBool("Jump",true);
        Transform chosenWall = wallPositions[Random.Range(0, wallPositions.Length)];
        Vector3 dir = (chosenWall.position - transform.position).normalized;
        dir.y = 1f;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * jumpForce, ForceMode.Impulse);
        Debug.Log("Wallmaster salta hacia la pared.");

        // Espera a que “llegue” a la pared (puedes ajustar este valor)
        yield return new WaitForSeconds(2f);
        

        // Se detiene y espera sobre la pared
        animator.SetBool("Jump",false);
        animator.SetBool("Wait",true);
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.4f); // Pausa en la pared antes de lanzarse

        // Ataque desde la pared
        animator.SetBool("Wait",false);
        animator.SetBool("Attack",true);
        Vector3 attackDir = (player.position - transform.position).normalized;
        attackDir.y = 0.2f;
        rb.AddForce(attackDir.normalized * jumpForce, ForceMode.Impulse);

        Debug.Log("Wallmaster se lanza desde la pared al jugador.");
        isInWallState = false;
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack",false);

    }

    IEnumerator GroundCharge()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * chargeForce, ForceMode.Impulse);
        animator.SetBool("Attack",true);
        Debug.Log("Wallmaster realiza una embestida en el suelo.");
        yield return new WaitForSeconds(1f);
        
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
