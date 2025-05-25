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
    private bool Light;

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
        animator.SetBool("LightMode", false);
        Light = false;
        ApplyFormVisuals();
    }

    void Update()
    {
        if (playerStats == null) return;

        // Iniciar conteo al mantener pulsado Q
        if (Input.GetKey(KeyCode.Q))
        {
            /*animator.SetBool("Transform",true);*/
            holdTime += Time.deltaTime;

            if (!hasTransformed && holdTime >= holdThreshold)
            {
                
                if(Light == true)
                {
                    animator.SetBool("LightMode", false);
                    Light = false;
                    
                }
                else
                {
                    animator.SetBool("LightMode", true);
                    Light = true;
                }
                CycleForm();
                hasTransformed = true;
            }
            
           
        }

        // Si suelta Q, se reinicia
        if (Input.GetKeyUp(KeyCode.Q))
        {
            /*animator.SetBool("Transform",false);*/
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

    
    Vector3 currentScale = playerTransform.localScale;
    bool isFacingRight = PlayerMovement3D.isFacingRight;

    currentScale.x = Mathf.Abs(currentScale.x) * (isFacingRight ? 1 : -1);
    playerTransform.localScale = currentScale;

    Debug.Log($"Forma actual: {playerStats.currentForm}");
}
}
