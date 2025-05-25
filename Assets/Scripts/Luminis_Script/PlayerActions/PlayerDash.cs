using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    private Rigidbody rb;
    private bool isDashing;
    private float dashEndTime;
    private float lastDashTime;
    private int dashDirection = 1; // -1 = izquierda, 1 = derecha
    private Animator animator;

    private PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on the player object.");
        }
    }

    void Update()
    {
        if (playerStats == null) return;

        float dashForce = playerStats.ActiveStats.dashForce;
        float dashDuration = playerStats.ActiveStats.dashDuration;
        float dashCooldown = playerStats.ActiveStats.dashCooldown;

        if (isDashing)
        {
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
                if (animator != null)
                {
                    animator.SetBool("IsDashing", false);
                }
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= lastDashTime + dashCooldown)
        {
            float input = Input.GetAxisRaw("Horizontal");
            if (input != 0)
            {
                dashDirection = (int)Mathf.Sign(input);
            }

            if (dashDirection != 0)
            {
                StartDash(dashDirection, dashForce, dashDuration);
            }
        }
    }

    void StartDash(int direction, float dashForce, float dashDuration)
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        lastDashTime = Time.time;

        Vector3 dashForceVector = new Vector3(direction * dashForce, 0f, 0f);
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // Reset X velocity before applying force
        rb.AddForce(dashForceVector, ForceMode.VelocityChange);
        Debug.Log("DASH aplicado: " + dashForceVector);

        if (animator != null)
        {
            animator.SetBool("IsDashing", true);
        }
    }
}