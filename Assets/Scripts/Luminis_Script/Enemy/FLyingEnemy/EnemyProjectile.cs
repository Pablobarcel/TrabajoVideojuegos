using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && !health.IsInvisible())
            {
                health.TakeDamage(damage, transform.position);
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
