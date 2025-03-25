using UnityEngine;

public class PendulumTorque : MonoBehaviour
{
    public float torqueStrength = 5f; // Fuerza del torque
    public ForceMode forceMode = ForceMode.Force; // Modo de aplicación del torque

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el Rigidbody
        if (rb == null)
        {
            Debug.LogError("No se encontró un Rigidbody en " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        // Aplicar torque en el eje Z para oscilar
        rb.AddTorque(Vector3.forward * torqueStrength, forceMode);
    }
}
