using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DatosPlayer : MonoBehaviour
{
    public int vidaPlayer;
    public Slider vidaVisual;

    // Pantalla de Game Over
    public GameObject pantallaGameOver;

    // Tiempo antes de reiniciar la escena
    public float tiempoRespawn = 5f;

    // Para controlar si el jugador ya está muerto
    private bool estaMuerto = false;

    void Start()
    {
        // Asegurarse de que la pantalla Game Over esté desactivada al inicio
        if (pantallaGameOver != null)
        {
            pantallaGameOver.SetActive(false);
        }
    }

    public void Update()
    {
        // Actualizar el slider de vida
        vidaVisual.value = vidaPlayer;

        // Verificar si el jugador ha muerto
        if (vidaPlayer <= 0 && !estaMuerto)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        estaMuerto = true;
        Debug.Log("GAME OVER");

        // Mostrar la pantalla de Game Over
        if (pantallaGameOver != null)
        {
            pantallaGameOver.SetActive(true);
        }

        // Desactivar componentes que controlan al jugador si es necesario
        // Por ejemplo: GetComponent<MovimientoJugador>().enabled = false;

        // Esperar 5 segundos y reiniciar la escena
        Invoke("ReiniciarEscena", tiempoRespawn);
    }

    void ReiniciarEscena()
    {
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Método opcional para recibir daño
    public void RecibirDano(int cantidad)
    {
        if (!estaMuerto)
        {
            vidaPlayer -= cantidad;
            // Asegurarse de que no baje de 0
            if (vidaPlayer < 0)
            {
                vidaPlayer = 0;
            }
        }
    }

    // Método opcional para curar
    public void Curar(int cantidad)
    {
        if (!estaMuerto)
        {
            vidaPlayer += cantidad;
        }
    }
}