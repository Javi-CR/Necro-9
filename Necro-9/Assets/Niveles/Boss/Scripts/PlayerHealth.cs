using UnityEngine;
using UnityEngine.UI; // Para UI si decides a�adir una barra de salud

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Opcional: Para mostrar la salud en UI
    public Slider healthSlider;

    // Opcional: Efecto cuando el jugador recibe da�o
    public GameObject damageEffect;

    void Start()
    {
        currentHealth = maxHealth;

        // Configurar slider de salud si existe
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jugador recibi� " + damage + " de da�o. Salud restante: " + currentHealth);

        // Actualizar UI si existe
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Mostrar efecto si existe
        if (damageEffect != null)
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("El jugador ha muerto");

        // Implementa aqu� lo que sucede cuando el jugador muere
        // Por ejemplo:
        // 1. Mostrar pantalla de game over
        // 2. Reiniciar nivel
        // 3. Animaci�n de muerte

        // Por ahora, simplemente desactivamos el GameObject
        gameObject.SetActive(false);
    }

    // M�todo para curar al jugador (si necesitas a�adir power-ups de salud)
    public void Heal(int amount)
    {
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