using UnityEngine;

public class MoverEsfera : MonoBehaviour
{
    public float velocidad = 5f; 

    void Update()
    {
        
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }
}