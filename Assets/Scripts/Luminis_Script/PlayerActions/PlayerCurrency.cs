using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    private UIManager ui;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats no encontrado en el jugador.");
            return;
        }

        ui = FindFirstObjectByType<UIManager>();
        if (ui == null)
        {
            Debug.LogError("UIManager no encontrado en la escena.");
            return;
        }

        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        playerStats.monedas += amount;
        UpdateUI();
    }

    public int GetCoins()
    {
        return playerStats.monedas;
    }

    private void UpdateUI()
    {
        ui?.UpdateMonedas(playerStats.monedas);
    }
}
