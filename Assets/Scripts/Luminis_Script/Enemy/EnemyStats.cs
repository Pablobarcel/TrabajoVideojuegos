using UnityEngine;

[System.Serializable]
public class EnemyStats : MonoBehaviour
{
    [Header("Combate")]
    public int damage = 1;
    public int coinReward = 5;

    [Header("Velocidades")]
    public float patrolSpeed = 3f;
    public float chaseSpeed = 8f;

    [Header("Vidas")]
    public int lifes = 2;

    [Header("Furia")]
    public int furyRewardOnDeath = 25;
    public int furyPerHit = 5;

}
