using UnityEngine;

public class DashUnlockPickup : MonoBehaviour
{
    public int monedasExtra = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.dashUnlocked)
            {
                stats.dashUnlocked = true;
                stats.AddMonedas(monedasExtra);
                Debug.Log("¡Dash desbloqueado!");

                // Puedes añadir un sonido o efecto visual aquí

                Destroy(gameObject); // Eliminar el pickup
            }
        }
    }
}
