using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    private bool isOpened = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isOpened) return;

        isOpened = true;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        Debug.Log("¡Cofre abierto!");
        // Aquí puedes añadir animaciones o efectos visuales
    }
}
