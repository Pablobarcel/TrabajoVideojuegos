using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform respawnPoint; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Solo afecta al jugador
        {
            Debug.Log($"{other.gameObject.name} Perdiste una vida. Reiniciando posición...");
            other.transform.position = respawnPoint.position; // Mueve el jugador al punto de reaparición
            other.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // Reinicia la velocidad para evitar arrastre
        }
    }
}
