using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public ChestReward reward; // ScriptableObject con la recompensa
    private bool isOpened = false;
    public Color openedColor = Color.gray;

    public GameObject coinPrefab; // Asignar desde el inspector
    public Transform spawnPoint;  // Desde dónde salen las monedas (puede ser el mismo cofre)
    public float spread = 0.5f;   // Para dispersión visual


    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Interact()
    {
        if (isOpened || reward == null) return;
        isOpened = true;

        // Spawnear monedas
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && coinPrefab != null)
        {
            for (int i = 0; i < reward.coinAmount; i++)
            {
                // Posición aleatoria cercana
                Vector3 offset = new Vector3(Random.Range(-spread, spread), 0.5f, Random.Range(-spread, spread));
                GameObject coin = Instantiate(coinPrefab, spawnPoint.position + offset, Quaternion.identity);

                Coin coinScript = coin.GetComponent<Coin>();
                if (coinScript != null)
                {
                    coinScript.SetTarget(player.transform);
                }
            }
        }


        // Cambiar color del cofre
        if (rend != null)
        {
            rend.material.color = openedColor;
        }
    }
}
