using UnityEngine;

public class HealingItem : MonoBehaviour
{
    private int healingAmount;

    public void SetHealing(int amount)
    {
        healingAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healingAmount);
            }

            Destroy(gameObject);
        }
    }
}
