using UnityEngine;
using UnityEngine.UI; // Para UI si decides añadir una barra de salud

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Opcional: Para mostrar la salud en UI
    public Slider healthSlider;

    // Opcional: Efecto cuando el jugador recibe daño
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
        Debug.Log("Jugador recibió " + damage + " de daño. Salud restante: " + currentHealth);

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

        // Implementa aquí lo que sucede cuando el jugador muere
        // Por ejemplo:
        // 1. Mostrar pantalla de game over
        // 2. Reiniciar nivel
        // 3. Animación de muerte

        // Por ahora, simplemente desactivamos el GameObject
        gameObject.SetActive(false);
    }

    // Método para curar al jugador (si necesitas añadir power-ups de salud)
    public void Heal(int amount)
    {
        currentHealth += amount;

        // Evitar que supere la salud máxima
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