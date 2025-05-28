using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("NameInput");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LoadGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
