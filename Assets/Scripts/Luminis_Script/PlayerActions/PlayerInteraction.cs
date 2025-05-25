using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.F;

    private InteractionPrompt interactionPrompt;
    private IInteractable currentInteractable;

    void Start()
    {
        interactionPrompt = FindAnyObjectByType<InteractionPrompt>();
    }

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);
        currentInteractable = null;

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                interactionPrompt.ShowPrompt("'F' Interact");
                return;
            }
        }

        // Si no hay ninguno cerca, ocultar mensaje
        interactionPrompt.HidePrompt();
    }
}
