using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour, IInteractable
{
    public string shopSceneName = "ShopScene"; // Nombre de la escena de la tienda

    public void Interact()
    {
        // Cargar la escena de la tienda
        SceneManager.LoadScene(shopSceneName);
    }
}
