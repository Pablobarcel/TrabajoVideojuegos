using UnityEngine;

public class FollowCameraY : MonoBehaviour
{
    public Transform cameraTransform; // Arrastra la cámara aquí en el Inspector

    void Update()
    {
        if (cameraTransform != null)
        {
            Vector3 currentPosition = transform.position;
            transform.position = new Vector3(currentPosition.x, cameraTransform.position.y, currentPosition.z);
        }
    }
}
