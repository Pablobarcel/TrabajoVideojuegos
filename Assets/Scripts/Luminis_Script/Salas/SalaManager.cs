using UnityEngine;

public class SalaManager : MonoBehaviour
{
    public static SalaManager Instance;

    private SalaController salaActual;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ActivarSala(SalaController nuevaSala)
    {
        if (salaActual != null && salaActual != nuevaSala)
            salaActual.OcultarSala();

        salaActual = nuevaSala;
        salaActual.MostrarSala();
    }
}
