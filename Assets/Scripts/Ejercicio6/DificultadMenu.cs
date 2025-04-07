using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ElegirDificultad(int vidas)
    {
        PlayerPrefs.SetInt("vidasIniciales", vidas);
        SceneManager.LoadScene("SampleScene");
    }
}
