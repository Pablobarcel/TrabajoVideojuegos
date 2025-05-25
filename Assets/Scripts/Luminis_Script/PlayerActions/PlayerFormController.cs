using UnityEngine;

public class PlayerFormController : MonoBehaviour
{
    private PlayerStats playerStats;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private Animator animator;

    private float holdTime = 0f;
    public float holdThreshold = 0.2f;
    private bool hasTransformed = false;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on the player object.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerTransform = transform;

        animator.SetBool("LightMode", playerStats.currentForm == PlayerStats.PlayerForm.Light);
        ApplyFormVisuals();
    }

    void Update()
    {
        if (playerStats == null || !playerStats.canChangeForm) return; // <-- aquí se comprueba

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

        if (Input.GetKeyUp(KeyCode.Q))
        {
            holdTime = 0f;
            hasTransformed = false;
        }
    }


    void CycleForm()
    {
        int oldMaxHealth = playerStats.ActiveStats.vidas;

        // Alternar entre Light y Shadow
        if (playerStats.currentForm == PlayerStats.PlayerForm.Light)
        {
            playerStats.currentForm = PlayerStats.PlayerForm.Shadow;
            animator.SetBool("LightMode", false);
        }
        else
        {
            playerStats.currentForm = PlayerStats.PlayerForm.Light;
            animator.SetBool("LightMode", true);
        }

        ApplyFormVisuals();
        Debug.Log($"Cambiando de forma a: {playerStats.currentForm}");

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

        Vector3 currentScale = playerTransform.localScale;
        bool isFacingRight = PlayerMovement3D.isFacingRight;
        currentScale.x = Mathf.Abs(currentScale.x) * (isFacingRight ? 1 : -1);
        playerTransform.localScale = currentScale;

        Debug.Log($"Forma actual: {playerStats.currentForm}");
    }
}
