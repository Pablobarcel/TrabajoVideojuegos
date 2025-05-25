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
                ToggleForm();  // <- Cambio aquí
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

    void ToggleForm()
    {
        int oldMaxHealth = playerStats.ActiveStats.vidas;
        PlayerStats.PlayerForm previousForm = playerStats.currentForm;

        // Cambiar entre Light y Shadow
        if (playerStats.currentForm == PlayerStats.PlayerForm.Light)
        {
            playerStats.currentForm = PlayerStats.PlayerForm.Shadow;
        }
        else
        {
            playerStats.currentForm = PlayerStats.PlayerForm.Light;
        }

        Debug.Log($"Cambiando de: {previousForm} a {playerStats.currentForm}");

        ApplyFormVisuals();

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

        Debug.Log($"Forma actual: {playerStats.currentForm}");
        // Aquí puedes aplicar visuales como color, sprite, efectos, etc.
    }
}
