using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip unlockSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.KeyUnlocked)
            {
                stats.KeyUnlocked = true;
                Debug.Log("¡Llave desbloqueada!");

                if (unlockSound)
                {
                    AudioSource.PlayClipAtPoint(unlockSound, transform.position);
                }

                Destroy(gameObject); // Elimina el consumible
            }
        }
    }
}
