using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartsContainer;

    public GameObject coinIconPrefab;
    public Transform coinContainer;
    public TextMeshProUGUI coinText;

    private List<GameObject> currentHearts = new List<GameObject>();

    public void UpdateVida(int vida)
    {
        foreach (GameObject heart in currentHearts)
            Destroy(heart);
        currentHearts.Clear();

        for (int i = 0; i < vida; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            currentHearts.Add(heart);
        }
    }

    public void UpdateMonedas(int monedas)
    {
        coinText.text = monedas.ToString();
    }
}
