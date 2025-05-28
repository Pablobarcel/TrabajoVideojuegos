using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LightWall : MonoBehaviour
{
    private Collider wallCollider;

    private void Awake()
    {
        wallCollider = GetComponent<Collider>();
        wallCollider.isTrigger = true; // Por defecto, es un trigger para permitir a Shadow pasar
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (stats != null && rb != null)
            {
                if (stats.currentForm != PlayerStats.PlayerForm.Light)
                {
                    // Activar colisión física (no trigger)
                    wallCollider.isTrigger = false;

                    // Empujar al jugador hacia atrás
                    Debug.Log("Jugador no puede atravesar Light Wall, empujando hacia atrás.");
                    Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                    pushDirection.y = 0; // Sin impulso vertical
                    float pushForce = 1000f;
                    rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerStats stats = collision.collider.GetComponent<PlayerStats>();
            if (stats != null && stats.currentForm != PlayerStats.PlayerForm.Light)
            {
                // Volver a dejarlo como trigger por si el jugador se aleja
                wallCollider.isTrigger = true;
            }
        }
    }
}

