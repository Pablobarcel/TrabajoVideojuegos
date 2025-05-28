using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public bool requiereLlave = true;
    public AudioClip abrirSonido;
    public AudioClip bloqueoSonido;

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats == null) return;

        if (!requiereLlave || stats.KeyUnlocked)
        {
            if (abrirSonido != null)
                AudioSource.PlayClipAtPoint(abrirSonido, transform.position);

            Debug.Log("Puerta abierta");
            Destroy(gameObject); // Destruye la puerta
        }
        else
        {
            if (bloqueoSonido != null)
                AudioSource.PlayClipAtPoint(bloqueoSonido, transform.position);

            Debug.Log("Necesitas una llave para abrir esta puerta.");
        }
    }
}
