using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 3, -5);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null) return;

        // La cámara se coloca detrás del jugador, respetando su rotación
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Mantiene la cámara mirando al jugador
        transform.LookAt(player);
    }
}
