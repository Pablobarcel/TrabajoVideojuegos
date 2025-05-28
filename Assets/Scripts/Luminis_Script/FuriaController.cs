using UnityEngine;
using UnityEngine.UI;

public class FuriaController : MonoBehaviour
{
    public Slider barraFuria;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats no encontrado.");
            enabled = false;
        }

        if (barraFuria == null)
        {
            Debug.LogError("Slider de furia no asignado.");
            enabled = false;
        }
    }

    void Update()
    {
        float porcentaje = playerStats.GetFuriaPorcentaje();
        barraFuria.value = porcentaje;
    }
}
