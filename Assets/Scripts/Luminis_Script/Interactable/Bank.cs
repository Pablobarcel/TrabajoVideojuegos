using UnityEngine;
using System.Collections.Generic;

public class Bank : MonoBehaviour, IInteractable
{
    public PlayerStats playerStats;

    private EnemyStats[] enemies;

    private void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        enemies = FindObjectsOfType<EnemyStats>();
    }

    public void Interact()
    {
        SaveGame();
        Debug.Log("Juego guardado en el banco.");
    }

    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData();

        // Guardar datos del jugador
        saveData.playerData = new PlayerSaveData(playerStats);

        // Guardar datos enemigos
        List<EnemySaveData> enemyDataList = new List<EnemySaveData>();
        foreach (var enemy in enemies)
        {
            enemyDataList.Add(new EnemySaveData(enemy));
        }
        saveData.enemiesData = enemyDataList.ToArray();

        // Serializar a JSON
        string json = JsonUtility.ToJson(saveData, true);

        // Guardar en PlayerPrefs
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SaveData"))
        {
            Debug.LogWarning("No hay datos guardados para cargar.");
            return;
        }

        // Aquí llamamos al LoadGame del playerStats, que ya tienes implementado
        playerStats.LoadGame();
        Debug.Log("Datos cargados correctamente.");
    }
}

// Clases para guardar

[System.Serializable]
public class GameSaveData
{
    public PlayerSaveData playerData;
    public EnemySaveData[] enemiesData;
}

[System.Serializable]
public class PlayerSaveData
{
    public int currentForm;
    public PlayerStats.FormStats lightStats;
    public PlayerStats.FormStats shadowStats;
    public int monedas;
    public int furiaActual;
    public Vector3 playerPosition;  // <-- aquí guardamos la posición

    public bool dashUnlocked;
    public bool HardAttackUnlocked;
    public bool SpecialAttackUnlocked;
    public bool KeyUnlocked;
    public bool wallJumpUnlocked;
    public bool canChangeForm;

    public int currentHealth;

    public PlayerSaveData(PlayerStats playerStats)
    {
        currentForm = (int)playerStats.currentForm;
        lightStats = playerStats.lightStats;
        shadowStats = playerStats.shadowStats;
        monedas = playerStats.monedas;
        furiaActual = playerStats.GetFuriaActual();

        dashUnlocked = playerStats.dashUnlocked;
        HardAttackUnlocked = playerStats.HardAttackUnlocked;
        SpecialAttackUnlocked = playerStats.SpecialAttackUnlocked;
        KeyUnlocked = playerStats.KeyUnlocked;
        wallJumpUnlocked = playerStats.wallJumpUnlocked;
        canChangeForm = playerStats.canChangeForm;

        PlayerHealth ph = playerStats.GetComponent<PlayerHealth>();
        currentHealth = ph != null ? ph.GetHealth() : playerStats.ActiveStats.vidas;

        playerPosition = playerStats.transform.position; // <-- guardamos la posición actual
    }
}


[System.Serializable]
public class EnemySaveData
{
    public string enemyID;
    public Vector3 position;
    public int health;

    public EnemySaveData(EnemyStats enemy)
    {
        enemyID = enemy.enemyID;
        position = enemy.transform.position;
        health = enemy.currentHealth;
    }
}
