using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; //Jugador
    public Vector3 offset = new Vector3(0, 3, -5); //Camara detras del jugador
    public float smoothSpeed = 5f; //Fluidez de la camara

    void LateUpdate()
    {
        if (player == null) return; //si no encuentra objeto player

        //Calcular la posicion deseada
        Vector3 desiredPosition = player.position + offset;

        //Interpolar suavemente la posicion de la camara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        //Camara mira siempre al jugador
        transform.LookAt(player);
    }
}
