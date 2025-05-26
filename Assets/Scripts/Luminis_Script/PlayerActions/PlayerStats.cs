using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum PlayerForm { Light , Shadow }
    public PlayerForm currentForm = PlayerForm.Light;

    [Header("Habilidades desbloqueadas")]
    public bool dashUnlocked = false;
    public bool HardAttackUnlocked = false;
    public bool SpecialAttackUnlocked = false;
    public bool KeyUnlocked = false;
    public bool wallJumpUnlocked = false;
    public bool canChangeForm = false; // Por defecto no puede cambiar

    [Header("Referencias")]
    public GameObject player;

    [System.Serializable]
    public class FormStats
    {
        [Header("Movimiento")]
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public int maxJumps = 2;

        [Header("Wall Jump / Slide")]
        public float wallSlideSpeed = 1f;
        public float wallJumpForce = 6f;

        [Header("Dash")]
        public float dashForce = 12f;
        public float dashDuration = 0.2f;
        public float dashCooldown = 1f;

        [Header("Ataque")]
        public int attackDamage = 1;
        public float attackSpeed = 1f;
        public float attackRange = 2f;

        [Header("Ataque Fuerte")]
        public int HardAttackDamage = 2;
        public float HardAttackSpeed = 2f;
        public float HardAttackRange = 1f;

        [Header("Ataque Especial")]
        public int SpecialAttackDamage = 2;
        public float SpecialAttackRange = 3f;
        public float SpecialAttackFallSpeed = -50f;
        public int SpecialAttackFuryCost = 25;

        [Header("Visual Settings")]
        public float scale = 1f;
        public Color color = Color.white;

        [Header("Otros")]
        public int vidas = 3;
        public int monedas = 0;

        [Header("Furia")]
        public int maxFuria = 100;

    }

    [Header("Stats por forma")]
    public FormStats shadowStats;
    public FormStats lightStats;

    [Header("Monedas del jugador (globales)")]
    public int monedas = 0;

    // Furia actual
    [Header("Furia actual")]
    [SerializeField] private int furiaActual = 0;

    // Acceso dinámico a la forma activa
    public FormStats ActiveStats
    {
        get
        {
            switch (currentForm)
            {
                case PlayerForm.Shadow: return shadowStats;
                case PlayerForm.Light: return lightStats;
                default: return lightStats;
            }
        }
    }

    // Métodos útiles de acceso
    public float GetMoveSpeed() => ActiveStats.moveSpeed;
    public float GetJumpForce() => ActiveStats.jumpForce;
    public int GetMaxJumps() => ActiveStats.maxJumps;
    public float GetDashForce() => ActiveStats.dashForce;
    public int GetAttackDamage() => ActiveStats.attackDamage;
    public int GetHardAttackDamage() => ActiveStats.HardAttackDamage;
    public int GetSpecialAttackDamage() => ActiveStats.SpecialAttackDamage;
    public int GetVidas() => ActiveStats.vidas;
    public int GetMonedas() => monedas;
    public void AddMonedas(int amount) => monedas += amount;

    // FURIA
    public int GetFuriaActual() => furiaActual;
    public int GetMaxFuria() => ActiveStats.maxFuria;

    public void AddFuria(int amount)
    {
        furiaActual = Mathf.Clamp(furiaActual + amount, 0, GetMaxFuria());
        // Aquí puedes llamar un evento para actualizar la UI
    }

    public void ConsumeFuria(int amount)
    {
        furiaActual = Mathf.Clamp(furiaActual - amount, 0, GetMaxFuria());
        // Aquí puedes llamar un evento para actualizar la UI
    }

    public void SetFuria(int value)
    {
        furiaActual = Mathf.Clamp(value, 0, GetMaxFuria());
    }

    public float GetFuriaPorcentaje()
    {
        return (float)furiaActual / GetMaxFuria();
    }
}
