using UnityEngine;
using TMPro;

public class LayerPortalZone : MonoBehaviour
{
    public bool PlayerInside { get; private set; }
    public GameObject hintText; // Arrastra el texto de UI aquí en el inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInside = true;
            if (hintText != null)
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
        }
    }
}
