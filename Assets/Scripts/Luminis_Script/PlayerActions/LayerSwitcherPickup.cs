using UnityEngine;

public class LayerSwitcherPickup : MonoBehaviour
{
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

            Destroy(gameObject); // Destruir el objeto recolectable
        }
    }
}
