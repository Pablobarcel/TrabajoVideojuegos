
using UnityEngine;

public class HardAttackPickup : MonoBehaviour
{
    public AudioClip unlockSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.HardAttackUnlocked)
            {
                stats.HardAttackUnlocked = true;
                Debug.Log("¡Ataque fuerte desbloqueado!");

                if (unlockSound)
                {
                    AudioSource.PlayClipAtPoint(unlockSound, transform.position);
                }

                Destroy(gameObject); // Elimina el consumible
            }
        }
    }
}
