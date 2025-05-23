using UnityEngine;
using TMPro;

public class GameStartController : MonoBehaviour
{
    public TMP_Text welcomeText;

    void Start()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Jugador");
        welcomeText.text =  name;
    }
}
