using UnityEngine;
using UnityEngine.UI;

public class CamaraSwitch : MonoBehaviour
{
    public Camera[] cameras; 
    public Button[] buttons; 

    void Start()
    {
        
        if (cameras.Length != buttons.Length)
        {
            Debug.LogError("El número de cámaras y botones debe ser igual.");
            return;
        }

        
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ActivateCamera(index));
        }

        
        ActivateCamera(0);
    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == index);
        }
    }
}

