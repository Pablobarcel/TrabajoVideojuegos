using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    private Vector3 forceDirection = Vector3.up;
    public float forceStrength = 10.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null) // Check if the object has a Rigidbody
        {
            Debug.Log($"{other.gameObject.name} entered the trigger zone!");

            // Calculate the force to apply
            Vector3 appliedForce = forceDirection.normalized * forceStrength;

            // Apply force to the object's Rigidbody
            other.attachedRigidbody.AddForce(appliedForce, ForceMode.Impulse);

            Debug.Log($"Applied force: {appliedForce}");
        }
    }
}
