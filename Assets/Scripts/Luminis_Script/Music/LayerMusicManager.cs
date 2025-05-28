using UnityEngine;
using System.Collections;

public class LayerMusicManager : MonoBehaviour
{
    public Transform player;
    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    public AudioClip musicLayer0;
    public AudioClip musicLayer1;
    public AudioClip musicLayer2;
    public float fadeDuration = 1.5f;

    private int currentLayer = -1;
    private AudioSource currentSource;
    private AudioSource nextSource;

    void Start()
    {
        currentSource = audioSourceA;
        nextSource = audioSourceB;
    }

    void Update()
    {
        float z = player.position.z;
        int layer = -1;

        if (z < 10)
            layer = 0;
        else if (z < 30)
            layer = 1;
        else
            layer = 2;

        if (layer != currentLayer)
        {
            currentLayer = layer;
            StartCoroutine(FadeToLayer(layer));
        }
    }

    IEnumerator FadeToLayer(int layer)
    {
        AudioClip nextClip = null;
        switch (layer)
        {
            case 0: nextClip = musicLayer0; break;
            case 1: nextClip = musicLayer1; break;
            case 2: nextClip = musicLayer2; break;
        }

        nextSource.clip = nextClip;
        nextSource.volume = 0f;
        nextSource.Play();

        float time = 0f;
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            currentSource.volume = Mathf.Lerp(1f, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            yield return null;
        }

        currentSource.Stop();
        currentSource.volume = 1f;

        // Intercambia los papeles de las fuentes
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}
