using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Reward", menuName = "Recompensas/Cofre")]
public class ChestReward : ScriptableObject
{
    [Header("Monedas")]
    public int coinAmount = 10;

    [Header("Curación")]
    public int healingItemsToSpawn = 0;
    public int healingAmountPerItem = 1;
}
