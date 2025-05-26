using UnityEngine;

public class WallJumpPickup : MonoBehaviour
{
    public int monedasExtra = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.wallJumpUnlocked)
            {
                stats.wallJumpUnlocked = true;
                stats.AddMonedas(monedasExtra);
                Debug.Log("¡Wall Jump desbloqueado!");

                // Puedes añadir un sonido o efecto visual aquí

                Destroy(gameObject); // Eliminar el pickup
            }
        }
    }
}
