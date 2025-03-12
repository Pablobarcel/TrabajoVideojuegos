using UnityEngine;

public class ZombieVision : MonoBehaviour
{
    public Transform player; //Jugador
    public float fieldOfViewAngle = 180f; //Vision del zombie
    public float moveSpeed = 3f; //Velocidad del zombi
    public float rotationSpeed = 5f; //Velocidad de rotacion del zombie

    private Rigidbody rb; //Referencia al rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;//direccion con vector normalizado
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer); //Producto vectorial desde donde mira el zombie hasta la posicion del jugador
        float angleThreshold = Mathf.Cos(fieldOfViewAngle * 0.5f * Mathf.Deg2Rad); //Convierte en coseno para comparar con dotProduct

        //Raycast para comprobar obstaculos
        RaycastHit hit;
        bool hasObstacle = Physics.Raycast(transform.position + Vector3.up * 1.5f, directionToPlayer, out hit, Vector3.Distance(transform.position, player.position));

        //Comprobar si el zombi ve al jugador
        if (dotProduct >= angleThreshold && (!hasObstacle || hit.collider.CompareTag("Player")))
        {
            Debug.Log("El zombi puede ver al jugador.");

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer); //Crea rotacion del zombie al jugador
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); //Hace que la rotacion sea mas suave

            //Moverse hacia el jugador
            rb.linearVelocity = transform.forward * moveSpeed;
        }
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        //direccion frontal del zombi
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 3);

        //dibujar los limites de vision
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftLimit * 3);
        Gizmos.DrawRay(transform.position, rightLimit * 3);
    }
}
