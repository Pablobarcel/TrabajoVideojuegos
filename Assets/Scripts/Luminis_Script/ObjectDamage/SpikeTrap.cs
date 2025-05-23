using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float knockbackForce = 1000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplicar daño
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(1, transform.position);
            }
        }
    }
}
