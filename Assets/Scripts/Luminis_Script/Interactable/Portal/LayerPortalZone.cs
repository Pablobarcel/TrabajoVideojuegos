using UnityEngine;

public class LayerPortalZone : MonoBehaviour
{
    public bool PlayerInside { get; private set; }
    public GameObject hintText; // Arrastra el texto de UI aquí en el inspector

    private PlayerLayerSwitcher playerLayerSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = true;
            playerLayerSwitcher = other.GetComponent<PlayerLayerSwitcher>();

            if (hintText != null && playerLayerSwitcher != null && playerLayerSwitcher.canSwitchLayers)
                hintText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = false;
            if (hintText != null)
                hintText.SetActive(false);

            playerLayerSwitcher = null;
        }
    }

    private void Update()
    {
        if (hintText != null && playerLayerSwitcher != null)
        {
            // Asegura que el texto esté activo solo si puede cambiar capas
            hintText.SetActive(PlayerInside && playerLayerSwitcher.canSwitchLayers);
        }
    }
}
