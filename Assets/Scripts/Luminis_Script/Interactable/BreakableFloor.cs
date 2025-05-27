using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    public GameObject breakEffect; // Opcional: efecto visual al romperse

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
            Debug.Log($"{playerAttack != null}");
            if (playerAttack != null && playerAttack.IsPerformingSpecialAttack)
            {
                Debug.Log("¡El suelo ha sido destruido por el ataque especial!");

                if (breakEffect != null)
                    Instantiate(breakEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
