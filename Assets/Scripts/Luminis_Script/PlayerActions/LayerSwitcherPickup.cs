using UnityEngine;

public class LayerSwitcherPickup : MonoBehaviour
{
    public int monedasExtra = 0;
    public AudioClip unlockSound;
    public GameObject CanvasPrefab;
    public float canvasDuration = 10f;
    public float destroyDelay = 0.1f; // Para permitir que el sonido se escuche

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ✅ Habilitar cambio de capa
            PlayerLayerSwitcher switcher = other.GetComponent<PlayerLayerSwitcher>();
            if (switcher != null)
            {
                switcher.canSwitchLayers = true;
                Debug.Log("¡Recogiste el artefacto de cambio de capas!");
            }

            // 🎧 Reproducir sonido de desbloqueo
            if (unlockSound != null)
            {
                AudioSource.PlayClipAtPoint(unlockSound, transform.position);
            }

            // 🖼️ Mostrar Canvas informativo
            if (CanvasPrefab != null)
            {
                GameObject canvas = Instantiate(CanvasPrefab);

                canvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
                canvas.transform.localScale = Vector3.one * 0.01f;

                Destroy(canvas, canvasDuration);
            }

            // 🕑 Destruir el objeto después de un pequeño retraso (para permitir que el sonido suene)
            Destroy(gameObject, destroyDelay);
        }
    }
}
