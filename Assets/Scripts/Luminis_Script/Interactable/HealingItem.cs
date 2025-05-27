using UnityEngine;

public class HealingItem : MonoBehaviour
{
    private int healingAmount;

    public void SetHealing(int amount)
    {
        healingAmount = amount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healingAmount);
            }

            Destroy(gameObject);
        }
    }
}
