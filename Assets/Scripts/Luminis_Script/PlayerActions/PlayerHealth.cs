using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private int currentHealth;
    private Vector3 startPosition;
    private UIManager ui;
    private PlayerStats playerStats;
    private Rigidbody rb;
    Animator animator;

    private bool isInvisible = false;
    private float invisibilityDuration = 3f;
    public float knockbackForce = 1000f;
    public GameOverManager gameOverManager;

    public Bank bank; // Referencia para cargar guardado

    void Start()
    {
        startPosition = transform.position;
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on the player object.");
            return;
        }

        ui = FindFirstObjectByType<UIManager>();
        if (ui == null)
        {
            Debug.LogError("UIManager not found in scene.");
        }

        if (bank == null)
        {
            bank = FindObjectOfType<Bank>();
            if (bank == null)
            {
                Debug.LogError("No se encontró el objeto Bank en la escena.");
            }
        }

        InitializeHealth();
    }

    void InitializeHealth()
    {
        currentHealth = playerStats.ActiveStats.vidas;
        ui?.UpdateVida(currentHealth);
    }

    public void TakeDamage(int damage, Vector3 damageSourcePosition)
    {
        if (isInvisible)
            return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        ui?.UpdateVida(currentHealth);

        StartCoroutine(BecomeInvisible());
        ApplyKnockback(damageSourcePosition);
        StartCoroutine(Ouch());

        if (currentHealth <= 0)
        {
            gameOverManager.TriggerGameOver();
            Die();
        }
    }

    private void ApplyKnockback(Vector3 damageSourcePosition)
    {
        if (rb == null) return;

        rb.linearVelocity = Vector3.zero;

        Vector3 knockbackDir = (transform.position - damageSourcePosition).normalized;
        Vector3 knockback = new Vector3(knockbackDir.x, knockbackDir.y, 0f) * knockbackForce;

        Debug.Log($"Fuerza Aplicada: {knockback}");

        rb.AddForce(knockback, ForceMode.Impulse);
    }

    private IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        yield return new WaitForSeconds(0.1f);

        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("InvisibleToEnemies");

        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
            renderer.material.color = new Color(1, 1, 1, 0.4f);

        yield return new WaitForSeconds(invisibilityDuration);

        gameObject.layer = originalLayer;

        if (renderer != null)
            renderer.material.color = new Color(1, 1, 1, 1f);

        isInvisible = false;
    }

    public IEnumerator Ouch()
    {
        animator.SetBool("Damage", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Damage", false);
    }

    public bool IsInvisible()
    {
        return isInvisible;
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto.");

        StopAllCoroutines();

        animator.SetBool("Damage", false);

        // Cargar datos guardados usando el método LoadGame() ya definido en playerStats
        if (bank != null)
        {
            bank.LoadGame();
            transform.position = playerStats.transform.position; // opcional si quieres mover al jugador al lugar guardado
        }
        else
        {
            // Si no hay bank, comportamiento por defecto
            transform.position = startPosition;
            currentHealth = playerStats.ActiveStats.vidas;
            ui?.UpdateVida(currentHealth);

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            isInvisible = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(1, 1, 1, 1f);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
        ui?.UpdateVida(currentHealth);
    }

    public void UpdateHealthOnFormChange(int oldMaxHealth)
    {
        int newMaxHealth = playerStats.ActiveStats.vidas;
        float healthRatio = oldMaxHealth > 0 ? (float)currentHealth / oldMaxHealth : 1f;
        currentHealth = Mathf.Clamp(Mathf.RoundToInt(newMaxHealth * healthRatio), 1, newMaxHealth);
        ui?.UpdateVida(currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, playerStats.ActiveStats.vidas);
        ui?.UpdateVida(currentHealth);
    }
}
