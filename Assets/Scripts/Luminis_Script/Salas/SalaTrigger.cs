using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SalaTrigger : MonoBehaviour
{
    public SalaController salaController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && salaController != null)
        {
            SalaManager.Instance.ActivarSala(salaController);
        }
    }
}
