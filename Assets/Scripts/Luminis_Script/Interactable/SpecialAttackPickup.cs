
using UnityEngine;

public class SpecialAttackPickup : MonoBehaviour
{
    public int monedasExtra = 0;
    public AudioClip unlockSound;
    public GameObject CanvasPrefab; // Asigna el prefab en el Inspector
    public float canvasDuration = 10f;

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

                // Muestra el Canvas si está asignado
                if (CanvasPrefab != null)
                {
                    GameObject canvas = Instantiate(CanvasPrefab);

                    // Colocarlo frente a la cámara si es World Space
                    canvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                    canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
                    canvas.transform.localScale = Vector3.one * 0.01f; // Ajusta si se ve muy grande

                    Destroy(canvas, canvasDuration);
                }

                Destroy(gameObject); // Elimina el consumible
            }
        }
    }
}
