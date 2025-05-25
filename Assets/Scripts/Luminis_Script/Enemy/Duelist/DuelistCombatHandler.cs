using UnityEngine;
using System.Collections;

public class DuelistCombatHandler : EnemyCombatHandler
{
    private int repeatedHits = 0;
    private float lastHitTime = 0f;
    private float repeatWindow = 2f;

    public GameObject coinPrefab;
    public GameObject dashUnlockPrefab;
    public Transform dropSpawnPoint;
    public float dropSpread = 0.5f;

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

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsInvisible())
            {
                playerHealth.TakeDamage(stats.damage, transform.position);
            }
        }

        stats.lifes -= damage;
        Debug.Log($"El duelista ha recibido {damage} de daño. Vida restante: {stats.lifes}");

        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.AddFuria(stats.furyPerHit);
        }

        if (stats.lifes <= 0 && !isDead)
        {
            isDead = true;

            if (playerStats != null)
            {
                playerStats.AddFuria(stats.furyRewardOnDeath);

                UIManager ui = FindFirstObjectByType<UIManager>();
                ui?.UpdateMonedas(playerStats.monedas);
            }

            // === Lanzar monedas ===
            if (coinPrefab != null && player != null)
            {
                for (int i = 0; i < stats.coinReward; i++)
                {
                    Vector3 offset = new Vector3(Random.Range(-dropSpread, dropSpread), 0.5f, Random.Range(-dropSpread, dropSpread));
                    GameObject coin = Instantiate(coinPrefab, dropSpawnPoint.position + offset, Quaternion.identity);

                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 launchDir = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
                        rb.AddForce(launchDir * 3f, ForceMode.Impulse);
                    }

                    Coin coinScript = coin.GetComponent<Coin>();
                    if (coinScript != null)
                    {
                        coinScript.SetTarget(player.transform);
                    }
                }
            }

            // === Lanzar dash unlock ===
            if (dashUnlockPrefab != null && player != null)
            {
                GameObject dashItem = Instantiate(dashUnlockPrefab, dropSpawnPoint.position, Quaternion.identity);

                Rigidbody rb = dashItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 launchDir = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f).normalized;
                    rb.AddForce(launchDir * 3f, ForceMode.Impulse);
                }
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
