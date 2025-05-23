using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NameInputController : MonoBehaviour
{
    public TMP_InputField nameInput;


    public void PlayGame()
    {
        string playerName = nameInput.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        SceneManager.LoadScene("Luminis_Scene");  // Asegúrate que esta escena exista
    }
}
