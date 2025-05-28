using UnityEngine;

public class FollowCameraYAndZ : MonoBehaviour
{
    public Transform cameraTransform; // Arrastra la cámara aquí en el Inspector

    void Update()
    {
        if (cameraTransform != null)
        {
            Vector3 currentPosition = transform.position;
            // Solo seguimos en Y y Z, dejamos la X como está para mantener la profundidad visual
            transform.position = new Vector3(currentPosition.x, cameraTransform.position.y, cameraTransform.position.z+60);
        }
    }
}
