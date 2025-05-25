using UnityEngine;

public class PlayerLayerSwitcher : MonoBehaviour
{
    [Header("Capas del mundo (en orden de Z)")]
    public float[] layerZPositions = { 0f, -20f, -40f };
    private int currentLayerIndex = 0;

    private LayerPortalZone currentPortal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentPortal != null && currentPortal.PlayerInside)
        {
            bool wantsDown = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            bool wantsUp = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

            if (wantsUp && currentLayerIndex > 0)
            {
                currentLayerIndex--;
                MoveToLayer(currentLayerIndex);
            }
            else if (wantsDown && currentLayerIndex < layerZPositions.Length - 1)
            {
                currentLayerIndex++;
                MoveToLayer(currentLayerIndex);
            }
        }
    }

    void MoveToLayer(int index)
    {
        Vector3 pos = transform.position;
        pos.z = layerZPositions[index];
        transform.position = pos;

        Debug.Log($"Jugador movido a capa {index} (Z = {layerZPositions[index]})");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LayerPortalZone>(out LayerPortalZone portal))
        {
            currentPortal = portal;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<LayerPortalZone>(out LayerPortalZone portal) && portal == currentPortal)
        {
            currentPortal = null;
        }
    }
}
