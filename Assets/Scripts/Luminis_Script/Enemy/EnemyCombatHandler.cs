using UnityEngine;
using System.Collections;

public class EnemyCombatHandler : MonoBehaviour
{
    protected EnemyStats stats;
    protected bool isDead = false;

    protected bool wasHitFromFront = true; // Se actualiza justo antes de TakeDamage

    protected virtual void Start()
    {
        stats = GetComponentInParent<EnemyStats>();
        if (stats == null)
        {
            Debug.LogError("EnemyStats no encontrado en el enemigo.");
        }
    }


    // NUEVO: se llama desde PlayerAttack para indicar la dirección del golpe
    public virtual void SetLastHitDirection(bool fromFront)
    {
        wasHitFromFront = fromFront;
    }

    public virtual void TakeDamage(int damage, GameObject player)
    {
        if (isDead) return;

        stats.lifes -= damage;
        Debug.Log($"El enemigo ha recibido {damage} de daño. Vida restante: {stats.lifes}");

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
                playerStats.AddFuria(stats.furyRewardOnDeath);

                UIManager ui = FindFirstObjectByType<UIManager>();
                ui?.UpdateMonedas(playerStats.monedas);
            }

            Destroy(transform.parent.gameObject);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

}
