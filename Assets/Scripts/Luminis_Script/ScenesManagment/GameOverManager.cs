using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas; // El canvas que muestras

    private bool isGameOver = false;

    // Llamar a este método para activar el Game Over
    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f; // Pausar el juego
        gameOverCanvas.SetActive(true);
    }

    // Opcional: para reiniciar el juego
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reanudar tiempo
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
