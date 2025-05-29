using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaInteractable : MonoBehaviour, IInteractable
{
    public string nombreEscena = "Luminis_Scene";

    public string GetInteractText()
    {
        return "Salir (F)";
    }

    public void Interact()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
