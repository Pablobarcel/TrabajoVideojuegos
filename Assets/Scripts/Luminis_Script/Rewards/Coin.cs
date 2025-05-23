using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform target;
    public float moveSpeed = 5f;

    public void SetTarget(Transform player)
    {
        target = player;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCurrency currency = other.GetComponent<PlayerCurrency>();
            if (currency != null)
            {
                currency.AddCoins(1);
            }

            Destroy(gameObject); // Destruir la moneda al recogerla
        }
    }
}
