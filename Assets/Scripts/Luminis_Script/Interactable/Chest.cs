using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public ChestReward reward;
    private bool isOpened = false;
    public Color openedColor = Color.gray;

    public GameObject coinPrefab;
    public GameObject healingItemPrefab;
    public Transform spawnPoint;
    public float spread = 0.5f;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (isOpened || reward == null) return;
        isOpened = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Soltar monedas
        if (player != null && coinPrefab != null)
        {
            for (int i = 0; i < reward.coinAmount; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-spread, spread), 0.5f, Random.Range(-spread, spread));
                GameObject coin = Instantiate(coinPrefab, spawnPoint.position + offset, Quaternion.identity);
                Coin coinScript = coin.GetComponent<Coin>();
                if (coinScript != null)
                {
                    coinScript.SetTarget(player.transform);
                }
            }
        }

        // Soltar objetos de curación
        if (healingItemPrefab != null)
        {
            for (int i = 0; i < reward.healingItemsToSpawn; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-spread, spread), 0.5f,0);
                GameObject heal = Instantiate(healingItemPrefab, spawnPoint.position + offset, Quaternion.identity);
                HealingItem healScript = heal.GetComponent<HealingItem>();
                if (healScript != null)
                {
                    healScript.SetHealing(reward.healingAmountPerItem);
                }
            }
        }

        if (rend != null)
        {
            rend.material.color = openedColor;
        }
    }
}
