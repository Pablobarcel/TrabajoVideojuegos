using UnityEngine;

public class ObjectTorque : MonoBehaviour
{
    public float torqueStrength = 5f; 
    public ForceMode forceMode = ForceMode.Force; //modo de aplicacion

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
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
