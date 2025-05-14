using UnityEngine;

public class LockCameraRotation : MonoBehaviour
{
    void Update()
    {
        // Solo permite rotación en Y
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
