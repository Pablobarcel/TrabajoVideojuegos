using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del jugador
    public float rotationSpeed = 10f; // Velocidad de rotación suave

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Dirección en espacio local del jugador
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection.magnitude > 0.1f) // Evitar rotaciones innecesarias
        {
            // Convertir dirección a coordenadas globales
            Vector3 worldDirection = transform.TransformDirection(moveDirection);

            // Aplicar movimiento
            rb.linearVelocity = new Vector3(worldDirection.x * moveSpeed, rb.linearVelocity.y, worldDirection.z * moveSpeed);

            // 🔥 Aplicar rotación suave con Lerp
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
