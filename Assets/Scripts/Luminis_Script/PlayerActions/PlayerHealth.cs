using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private int currentHealth;
    private UIManager ui;
    private PlayerStats playerStats;
    private Rigidbody rb;

    private bool isInvisible = false;
    private float invisibilityDuration = 3f;
    public float knockbackForce = 1000f; // Fuerza de empuje al recibir daño

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();

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

        InitializeHealth();
    }

    void InitializeHealth()
    {
        currentHealth = playerStats.ActiveStats.vidas;
        ui?.UpdateVida(currentHealth);
    }

    // Modificado para recibir posición del daño y aplicar knockback + invisibilidad
    public void TakeDamage(int damage, Vector3 damageSourcePosition)
    {
        if (isInvisible)
            return; // No recibe daño si está invisible

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        ui?.UpdateVida(currentHealth);

        ApplyKnockback(damageSourcePosition);
        StartCoroutine(BecomeInvisible());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(Vector3 damageSourcePosition)
    {
        if (rb == null) return;

        // Resetear velocidad del jugador
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

        // Efecto visual opcional
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
            renderer.material.color = new Color(1, 1, 1, 0.4f); // semitransparente

        yield return new WaitForSeconds(invisibilityDuration);

        // Restaurar layer original
        gameObject.layer = originalLayer;

        // Restaurar efecto visual
        if (renderer != null)
            renderer.material.color = new Color(1, 1, 1, 1f);

        isInvisible = false;
    }




    public bool IsInvisible()
    {
        return isInvisible;
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto.");
        // Reiniciar nivel, mostrar animación, etc.
    }

    public int GetHealth()
    {
        return currentHealth;
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
