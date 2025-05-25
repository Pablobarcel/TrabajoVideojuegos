using UnityEngine;

public class InteractiveDoor : MonoBehaviour, IInteractable
{
    [Header("Destino al interactuar")]
    public Transform targetPosition;

    [Header("Referencia al Duelist")]
    public EnemyCombatHandler duelistHandler;

    public void Interact()
    {
        if (duelistHandler != null && !duelistHandler.IsDead())
        {
            Debug.Log("La puerta está bloqueada hasta que derrotes al Duelista.");
            return;
        }

        if (targetPosition != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetPosition.position;
                Debug.Log("Puerta usada. Jugador teletransportado.");
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado una posición de destino.");
        }
    }
}
