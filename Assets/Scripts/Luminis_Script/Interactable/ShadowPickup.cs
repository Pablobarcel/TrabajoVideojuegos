using UnityEngine;

public class ShadowPickup : MonoBehaviour
{
    public AudioClip unlockSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.canChangeForm)
            {
                stats.canChangeForm = true;
                Debug.Log("¡Cambio de forma desbloqueado!");

                if (unlockSound)
                {
                    AudioSource.PlayClipAtPoint(unlockSound, transform.position);
                }

                Destroy(gameObject); // Elimina el consumible
            }
        }
    }
}
