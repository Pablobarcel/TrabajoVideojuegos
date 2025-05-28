using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour, IInteractable
{
    

    public void Interact()
    {
        // Cargar la escena de la tienda
        SceneManager.LoadScene("Tienda2");
    }
}
