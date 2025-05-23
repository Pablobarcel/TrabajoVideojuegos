using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI vidaText;
    public TextMeshProUGUI monedasText;

    public void UpdateVida(int vida)
    {
        vidaText.text = "Lifes: " + vida;
    }

    public void UpdateMonedas(int monedas)
    {
        monedasText.text = "Coins: " + monedas;
    }
}
