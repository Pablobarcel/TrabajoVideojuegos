using UnityEngine;

public class EnemyVisionTrigger : MonoBehaviour
{
    private EnemyMovementController movement;

    private void Start()
    {
        movement = GetComponentInParent<EnemyMovementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && movement != null)
        {
            // Si el jugador está en el layer InvisibleToEnemies, ignorarlo
            if (other.gameObject.layer == LayerMask.NameToLayer("InvisibleToEnemies"))
            {
                Debug.Log("Jugador invisible: no se persigue.");
                return;
            }

            movement.SetTarget(other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Si el jugador se vuelve invisible mientras ya estaba en rango
        if (other.CompareTag("Player") && movement != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("InvisibleToEnemies"))
            {
                Debug.Log("Jugador se volvió invisible en rango. Se detiene persecución.");
                movement.ClearTarget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && movement != null)
        {
            movement.ClearTarget();
        }
    }
}
