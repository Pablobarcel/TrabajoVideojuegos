
using UnityEngine;

public class SpecialAttackPickup : MonoBehaviour
{
    public AudioClip unlockSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.SpecialAttackUnlocked)
            {
                stats.SpecialAttackUnlocked = true;
                Debug.Log("¡Ataque especial desbloqueado!");

                if (unlockSound)
                {
                    AudioSource.PlayClipAtPoint(unlockSound, transform.position);
                }

                Destroy(gameObject); // Elimina el consumible
            }
        }
    }
}
