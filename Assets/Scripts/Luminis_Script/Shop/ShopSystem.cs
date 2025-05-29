using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public ShopItem[] items;
    public Button[] buyButtons;
    public TextMeshProUGUI[] priceTexts;

    private PlayerStats playerStats;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        playerHealth = FindObjectOfType<PlayerHealth>();

        for (int i = 0; i < items.Length; i++)
        {
            int index = i;
            priceTexts[i].text = items[i].price + " Coins";
            buyButtons[i].onClick.AddListener(() => BuyItem(index));
        }
    }

    public void BuyItem(int index)
    {
        if (index < 0 || index >= items.Length) return;

        ShopItem item = items[index];

        if (playerStats.GetMonedas() >= item.price)
        {
            playerStats.AddMonedas(-item.price);
            playerHealth.Heal(item.healingAmount);

            FindObjectOfType<UIManager>().UpdateMonedas(playerStats.GetMonedas());
            FindObjectOfType<UIManager>().UpdateVida(playerHealth.GetHealth());

            Debug.Log($"Comprado: {item.itemName} (+{item.healingAmount} vida)");
        }
        else
        {
            Debug.Log("No tienes suficientes monedas");
        }
    }
}
