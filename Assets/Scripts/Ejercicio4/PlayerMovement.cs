using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; //Velocidad del jugador
    private Rigidbody rb; //Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //Obtener el Rigidbody
        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en " + gameObject.name);
        }
    }

    void Update()
    {
        //Obtener entrada del teclado
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Crear dirección de movimiento
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized; //Creamos vector normalizado de movimiento para la direccion

        //Aplicar movimiento
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection; //Hace que el Player gire en la direccion del movimiento
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // Obtener la masa del objeto con el que colisiona
        Rigidbody otherRb = collision.rigidbody;

        if (otherRb != null)
        {
            float massDifference = otherRb.mass / rb.mass; // Relación de masas
            Vector3 impactForce = collision.relativeVelocity * otherRb.mass * 0.5f; // Calcula la fuerza del impacto

            // Si el objeto que golpea es más pesado, el player reacciona más
            if (massDifference >= 1)
            {
                rb.AddForce(impactForce, ForceMode.Force); // Desplazamiento realista
            }
        }
    }
}
 