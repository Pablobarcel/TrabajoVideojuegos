using UnityEngine;

public class SalaController : MonoBehaviour
{
    public GameObject paredOpaca; // Objeto que oculta la sala

    public void MostrarSala()
    {
        if (paredOpaca != null)
            paredOpaca.SetActive(false);
    }

    public void OcultarSala()
    {
        if (paredOpaca != null)
            paredOpaca.SetActive(true);
    }
}
