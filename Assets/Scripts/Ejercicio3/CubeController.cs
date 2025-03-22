using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;

    void Update()
    {
     
        // Movimiento en Update (afectado por FPS)
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime; //Time.deltaTime es 1/FPS, por eso hace que el movimiento sea independiente de la velocidad de la máquina
        transform.Translate(0, 0, move);

        // Rotación en Update
       // float rotate = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
      // transform.Rotate(0,rotationSpeed * Time.deltaTime , 0);
        
    }

    void FixedUpdate()
    {
       
        // Rotación en FixedUpdate para comparación
        transform.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0);
        
    }
}
