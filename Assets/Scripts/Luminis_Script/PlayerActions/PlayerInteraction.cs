using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Tecla F pulsada");
            Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    break; // Solo uno por tecla
                }
            }
        }
    }
}
