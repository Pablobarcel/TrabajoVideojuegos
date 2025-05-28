using UnityEngine;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    public GameObject shopCanvas; // Asigna aquí tu canvas de tienda en el Inspector

    private bool isShopOpen = false;

    private void Start()
    {
        if (shopCanvas != null)
        {
            shopCanvas.SetActive(false); // Asegurarse de que la tienda esté oculta al inicio
        }
    }

    public void Interact()
    {
        if (shopCanvas == null) return;

        isShopOpen = !isShopOpen;
        shopCanvas.SetActive(isShopOpen);

        // Pausar el juego si deseas (opcional)
        if (isShopOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isShopOpen = false;
            shopCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
    }

}
