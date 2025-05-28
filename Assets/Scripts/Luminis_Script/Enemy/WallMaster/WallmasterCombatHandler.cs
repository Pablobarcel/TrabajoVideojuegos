using UnityEngine;

public class WallmasterCombatHandler : EnemyCombatHandler
{
    public GameObject wallJumpUnlockPrefab;
    public GameObject wallPrefab;
    public GameObject coinPrefab;
    public Transform dropSpawnPoint;
    public float dropSpread = 0.5f;

    private Transform player;

    private void Startt()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        stats = GetComponentInParent<EnemyStats>();
    }

    public override void TakeDamage(int damage, GameObject player)
    {
        if (isDead) return;

        stats.lifes -= damage;
        Debug.Log($"Wallmaster recibe {damage} de daño. Vida restante: {stats.lifes}");

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
                    Vector3 offset = new Vector3(
                        Random.Range(-dropSpread, dropSpread),
                        0.5f,
                        Random.Range(-dropSpread, dropSpread)
                    );

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

            // === Lanzar wall jump unlock ===
            if (wallJumpUnlockPrefab != null && player != null)
            {
                GameObject unlock = Instantiate(wallJumpUnlockPrefab, dropSpawnPoint.position, Quaternion.identity);
                Rigidbody rb = unlock.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Vector3 launchDir = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f).normalized;
                    rb.AddForce(launchDir * 3f, ForceMode.Impulse);
                }
            }
            Destroy(wallPrefab);
            Destroy(transform.gameObject);
        }
    }
}
