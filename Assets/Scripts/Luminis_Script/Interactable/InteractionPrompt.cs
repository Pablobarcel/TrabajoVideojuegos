using UnityEngine;
using TMPro; // si usas TextMeshPro

public class InteractionPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;

    void Start()
    {
        HidePrompt();
    }

    public void ShowPrompt(string message = "'F' Interact")
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        promptText.gameObject.SetActive(false);
    }
}
