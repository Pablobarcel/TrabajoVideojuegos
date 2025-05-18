using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public enum PlayerForm { Normal, Light, Shadow }
    public PlayerForm currentForm = PlayerForm.Normal;


    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleForm();
        }

        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void CycleForm()
    {
        currentForm = (PlayerForm)(((int)currentForm + 1) % 3);
        ApplyFormEffects();
    }

    void ApplyFormEffects()
    {
        switch (currentForm)
        {
            case PlayerForm.Normal:
                // Reset
                moveSpeed = 5f;
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case PlayerForm.Light:
                moveSpeed = 7f;
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case PlayerForm.Shadow:
                moveSpeed = 4f;
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }
    }

}
