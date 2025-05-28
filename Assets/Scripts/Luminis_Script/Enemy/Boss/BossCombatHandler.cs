using UnityEngine;
using System.Collections;

public class BossCombatHandler : EnemyCombatHandler
{
    public GameObject coinPrefab;
    public Transform dropPoint;
    public float dropSpread = 0.5f;
    Animator animator;

    

    public override void TakeDamage(int damage, GameObject player)
    {
        if (isDead) return;
        
       
        stats.lifes -= damage;
        Debug.Log($"Boss recibió {damage} de daño. Vida restante: {stats.lifes}");
        
         StartCoroutine(Ouch());
        PlayerStats ps = player.GetComponent<PlayerStats>();
        if (ps != null)
        {
            ps.AddFuria(stats.furyPerHit);
        }

        if (stats.lifes <= 0)
        {
            isDead = true;

            if (ps != null)
            {
                ps.AddFuria(stats.furyRewardOnDeath);
                ps.monedas += stats.coinReward;
                UIManager ui = FindFirstObjectByType<UIManager>();
                ui?.UpdateMonedas(ps.monedas);
            }

            // Drop de objeto especial
            if (coinPrefab != null && player != null)
            {
                for (int i = 0; i < stats.coinReward; i++)
                {
                    Vector3 offset = new Vector3(Random.Range(-dropSpread, dropSpread), 0.5f, Random.Range(-dropSpread, dropSpread));
                    GameObject coin = Instantiate(coinPrefab, dropPoint.position + offset, Quaternion.identity);

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

            Debug.Log("¡Boss derrotado!");
            Destroy(transform.gameObject);
        }
    }

    IEnumerator Ouch()
    {
        animator.SetBool("Ouch", true);
        yield return new WaitForSeconds(2f); 
        animator.SetBool("Ouch", false);
        
            
        
    }
}