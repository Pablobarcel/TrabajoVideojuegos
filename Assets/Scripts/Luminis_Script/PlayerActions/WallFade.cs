using UnityEngine;

public class SalaVisibilityTrigger : MonoBehaviour
{
    public GameObject paredSala; // Pared opaca que oculta la sala completa

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            paredSala.SetActive(false); // Hace visible la sala entera
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            paredSala.SetActive(true); // (Opcional) Vuelve a ocultar la sala al salir
        }
    }
}