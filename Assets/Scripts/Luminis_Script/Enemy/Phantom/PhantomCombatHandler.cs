using UnityEngine;

public class PhantomCombatHandler : EnemyCombatHandler
{
    public GameObject coinPrefab;
    public GameObject SpecialAttackUnlockPrefab;
    public Transform dropSpawnPoint;
    public float dropSpread = 0.5f;

    public override void TakeDamage(int damage, GameObject player)
    {
        if (isDead) return;

        stats.lifes -= damage;
        Debug.Log($"Phantom recibió {damage} de daño. Vida restante: {stats.lifes}");

        PlayerStats ps = player.GetComponent<PlayerStats>();
        if (ps != null)
        {
            ps.AddFuria(stats.furyPerHit);
        }

        if (stats.lifes <= 0 && !isDead)
        {
            isDead = true;

            if (ps != null)
            {
                ps.AddFuria(stats.furyRewardOnDeath);

                UIManager ui = FindFirstObjectByType<UIManager>();
                ui?.UpdateMonedas(ps.monedas);
            }

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

            if (SpecialAttackUnlockPrefab != null && player != null)
            {
                GameObject specialItem = Instantiate(SpecialAttackUnlockPrefab, dropSpawnPoint.position, Quaternion.identity);

                Rigidbody rb = specialItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 launchDir = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f).normalized;
                    rb.AddForce(launchDir * 3f, ForceMode.Impulse);
                }
            }

            Destroy(transform.gameObject);
        }
    }
}
