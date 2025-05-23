using UnityEngine;

public class LayerSwitcher : MonoBehaviour
{
    public float[] layerZPositions = { 0f, -20f, -40f };
    private int currentLayer = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentLayer = (currentLayer + 1) % layerZPositions.Length;
            Vector3 pos = transform.position;
            pos.z = layerZPositions[currentLayer];
            transform.position = pos;
        }
    }
}
