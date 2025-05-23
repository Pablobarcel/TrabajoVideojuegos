using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public AudioClip hoverClip;
    public AudioClip clickClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();
    }

    public void PlayHover() => audioSource.PlayOneShot(hoverClip);
    public void PlayClick() => audioSource.PlayOneShot(clickClip);
}
