using UnityEngine;

public class DashUnlockPickup : MonoBehaviour
{
    public int monedasExtra = 0;
    public GameObject canvasPrefab; // Asigna esto en el Inspector
    public float canvasDuration = 10f; // Tiempo que permanece visible el Canvas

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null && !stats.dashUnlocked)
            {
                stats.dashUnlocked = true;
                stats.AddMonedas(monedasExtra);
                Debug.Log("¡Dash desbloqueado!");

                if (canvasPrefab != null)
                {
                    GameObject canvas = Instantiate(canvasPrefab);

                    // Si es World Space, lo colocamos frente a la cámara
                    canvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                    canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
                    canvas.transform.localScale = Vector3.one * 0.01f;

                    // Destruir el canvas tras un retardo
                    Destroy(canvas, canvasDuration);
                }

                Destroy(gameObject); // Eliminar el objeto pickup
            }
        }
    }
}
