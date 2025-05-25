using UnityEngine;
using System.Collections;

public class DuelistCombatHandler : EnemyCombatHandler
{
    private int repeatedHits = 0;
    private float lastHitTime = 0f;
    private float repeatWindow = 2f;

    public override void TakeDamage(int damage, GameObject player)
    {
        if (isDead) return;

        float now = Time.time;
        if (now - lastHitTime < repeatWindow)
            repeatedHits++;
        else
            repeatedHits = 1;

        lastHitTime = now;

        if (wasHitFromFront && Random.value < 0.4f)
        {
            Debug.Log("¡Parry del duelista! No recibe daño.");
            return;
        }

        if (repeatedHits >= 3)
        {
            Debug.Log("¡El duelista contraataca!");
            repeatedHits = 0;

            // Aquí hacemos daño real al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsInvisible())
            {
                playerHealth.TakeDamage(stats.damage, transform.position);
            }
        }


        base.TakeDamage(damage, player);
    }
}
