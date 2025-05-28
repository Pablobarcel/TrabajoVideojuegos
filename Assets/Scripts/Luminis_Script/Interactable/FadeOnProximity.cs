using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeOnProximity : MonoBehaviour
{
    public Transform player;
    public float visibleDistance = 10f;
    private Material mat;
    private Color originalColor;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        float alpha = Mathf.Clamp01(1f - (dist / visibleDistance));

        Color newColor = originalColor;
        newColor.a = alpha;
        mat.color = newColor;
    }
}
