using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameController : MonoBehaviour
{
    public void CargarPartida1()
    {
        SceneManager.LoadScene("Luminis_Scene"); // Cambia a tu escena real
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
