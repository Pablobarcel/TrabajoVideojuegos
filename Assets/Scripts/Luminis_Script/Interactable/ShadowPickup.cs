using UnityEngine;

public class ShadowPickup : MonoBehaviour
{
    public AudioClip unlockSound;
    public GameObject sombraCanvasPrefab; // Asigna el prefab en el Inspector
    public float canvasDuration = 10f; // Tiempo que permanece visible

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.canChangeForm)
            {
                stats.canChangeForm = true;
                Debug.Log("¡Cambio de forma desbloqueado!");

                // Reproduce el sonido de desbloqueo
                if (unlockSound)
                {
                    AudioSource.PlayClipAtPoint(unlockSound, transform.position);
                }

                // Muestra el Canvas si está asignado
                if (sombraCanvasPrefab != null)
                {
                    GameObject canvas = Instantiate(sombraCanvasPrefab);

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
