using UnityEngine;

public class Pendulo : MonoBehaviour
{
    public float fuerzaInicial = 100f; 

    private Rigidbody rb;

    void Start()
    {
            rb = GetComponent<Rigidbody>();

            //me aseguro de que el rg esta bien configurado
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddForce(Vector3.right * fuerzaInicial, ForceMode.Impulse);
    }
}
