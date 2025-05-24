using UnityEngine;

public class HealingPickup : MonoBehaviour
{
    public int healingAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healingAmount);
                Destroy(gameObject);
            }
        }
    }
}
