using UnityEngine;

public class PlayerFormController : MonoBehaviour
{
    private PlayerStats playerStats;

    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    private float holdTime = 0f;
    public float holdThreshold = 2f;
    private bool hasTransformed = false;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on the player object.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = transform;

        ApplyFormVisuals();
    }

    void Update()
    {
        if (playerStats == null) return;

        // Iniciar conteo al mantener pulsado Q
        if (Input.GetKey(KeyCode.Q))
        {
            holdTime += Time.deltaTime;

            if (!hasTransformed && holdTime >= holdThreshold)
            {
                CycleForm();
                hasTransformed = true;
            }
        }

        // Si suelta Q, se reinicia
        if (Input.GetKeyUp(KeyCode.Q))
        {
            holdTime = 0f;
            hasTransformed = false;
        }
    }

    void CycleForm()
    {
        // Cambiar forma en PlayerStats
        int oldMaxHealth = playerStats.ActiveStats.vidas;
        PlayerStats.PlayerForm previousForm = playerStats.currentForm;
        playerStats.currentForm = (PlayerStats.PlayerForm)(((int)playerStats.currentForm + 1) % 3);
        Debug.Log($"Cambiando de: {previousForm} a {playerStats.currentForm}");
        ApplyFormVisuals();
        // En PlayerFormController, dentro de CycleForm() justo después de ApplyFormVisuals():
        var playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.UpdateHealthOnFormChange(oldMaxHealth);
        }

    }

    void ApplyFormVisuals()
    {
        if (playerStats == null) return;

        var activeStats = playerStats.ActiveStats;
        playerTransform.localScale = Vector3.one * activeStats.scale;
        spriteRenderer.color = activeStats.color;

        Debug.Log($"Forma actual: {playerStats.currentForm}");
    }
}
