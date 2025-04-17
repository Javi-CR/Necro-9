using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Opcional: Para mostrar la salud en UI
    public Slider healthSlider;

    // Pantalla de Game Over
    public GameObject gameOverScreen;

    // Tiempo antes de reiniciar la escena
    public float respawnTime = 5f;

    // Controla si el jugador est� muerto
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Configurar slider de salud si existe
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // Asegurarse de que la pantalla Game Over est� desactivada al inicio
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        // No procesar da�o si ya est� muerto
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log("Jugador recibi� " + damage + " de da�o. Salud restante: " + currentHealth);

        // Actualizar UI si existe
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Evitar llamadas m�ltiples
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Game Over");

        // Mostrar pantalla de Game Over
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Programar el reinicio de la escena despu�s de X segundos
        Invoke("RestartScene", respawnTime);
    }

    void RestartScene()
    {
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // M�todo para curar al jugador
    public void Heal(int amount)
    {
        // No curar si est� muerto
        if (isDead)
            return;

        currentHealth += amount;

        // Evitar que supere la salud m�xima
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Actualizar UI si existe
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log("Jugador curado por " + amount + ". Salud actual: " + currentHealth);
    }
}