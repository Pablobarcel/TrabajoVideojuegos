using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour
{
    private EnemyStats stats;
    private bool isDead = false;

    private void Start()
    {
        stats = GetComponentInParent<EnemyStats>();
        if (stats == null)
        {
            Debug.LogError("EnemyStats no encontrado en el enemigo.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        PlayerAttack attack = other.GetComponent<PlayerAttack>();

        if (attack != null && attack.IsPerformingSpecialAttack)
        {
            Debug.Log("¡El jugador ha caído sobre el enemigo con ataque especial!");
            return; // No dañamos al jugador
        }

        if (health != null && !health.IsInvisible())
        {
            Debug.Log("¡Jugador en rango de ataque!");
            health.TakeDamage(stats.damage, transform.position);
        }
    }


    public void TakeDamage(int damage, GameObject player)
    {
        if (isDead) return;

        stats.lifes -= damage;
        Debug.Log($"El enemigo ha recibido {damage} de daño. Vida restante: {stats.lifes}");

        // Añadir furia al jugador por impacto
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.AddFuria(stats.furyPerHit);
        }

        if (stats.lifes <= 0)
        {
            isDead = true;
            Debug.Log("¡Enemigo derrotado!");

            if (playerStats != null)
            {
                playerStats.monedas += stats.coinReward;
                playerStats.AddFuria(stats.furyRewardOnDeath); // Dar furia por matar al enemigo

                UIManager ui = FindFirstObjectByType<UIManager>();
                ui?.UpdateMonedas(playerStats.monedas);
            }

            Destroy(transform.parent.gameObject);
        }
    }

}
