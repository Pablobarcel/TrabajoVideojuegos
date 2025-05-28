using UnityEngine;

public class LayerSwitcherPickup : MonoBehaviour
{
    public int monedasExtra = 0;
    public AudioClip unlockSound;
    public GameObject CanvasPrefab; // Asigna el prefab en el Inspector
    public float canvasDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLayerSwitcher switcher = other.GetComponent<PlayerLayerSwitcher>();
            if (switcher != null)
            {
                switcher.canSwitchLayers = true;
                Debug.Log("¡Recogiste el artefacto de cambio de capas!");
            }

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

            Destroy(gameObject); // Destruir el objeto recolectable
        }
    }
}
