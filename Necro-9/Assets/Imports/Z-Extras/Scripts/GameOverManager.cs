using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject gameOverUI;
    public Button retryButton;
    public Button menuButton;

    private void Start()
    {
        // Asegúrate que esté desactivado al comenzar
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        // Asigna funciones a los botones (si están)
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);

        if (menuButton != null)
            menuButton.onClick.AddListener(MainMenu);
    }

    public void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }
        else
        {
            Debug.LogWarning("No se asignó el GameOver UI.");
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        LevelLoader.LoadLevel("Nivel 2");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        LevelLoader.LoadLevel("Menu");
    }
}
