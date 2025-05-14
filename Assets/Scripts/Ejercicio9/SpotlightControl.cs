using UnityEngine;

public class SpotlightControl : MonoBehaviour
{
    public Light spotlight;  // La luz spotlight a controlar
    public KeyCode toggleKey = KeyCode.Space;  // La tecla para encender o apagar la luz

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))  // Si se presiona la tecla asignada
        {
            // Cambiar el estado de la luz (encender o apagar)
            spotlight.enabled = !spotlight.enabled;
        }
    }
}
