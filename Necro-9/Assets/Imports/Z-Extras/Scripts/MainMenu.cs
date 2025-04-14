using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Referencias a los botones
    public Button botonJugar;
    public Button botonSalir;

    // Al iniciar
    void Start()
    {
        // Buscar los botones automáticamente si no están asignados
        if (botonJugar == null)
        {
            botonJugar = GameObject.Find("BotonJugar")?.GetComponent<Button>();
        }

        if (botonSalir == null)
        {
            botonSalir = GameObject.Find("BotonSalir")?.GetComponent<Button>();
        }

        // Asignar los listeners a los botones
        if (botonJugar != null)
        {
            botonJugar.onClick.AddListener(Jugar);
            Debug.Log("Botón Jugar encontrado y conectado");
        }
        else
        {
            Debug.LogError("No se encontró el botón Jugar. Asegúrate de que existe y se llama 'BotonJugar'");
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.AddListener(Salir);
            Debug.Log("Botón Salir encontrado y conectado");
        }
        else
        {
            Debug.LogError("No se encontró el botón Salir. Asegúrate de que existe y se llama 'BotonSalir'");
        }
    }

    // Método para el botón Jugar
    public void Jugar()
    {
        Debug.Log("Intentando cargar el nivel 'Nivel 1'...");

        // Verificar si existe la clase LevelLoader
        try
        {
            LevelLoader.LoadLevel("Nivel 1");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar el nivel: " + e.Message);
            // Alternativa si LevelLoader no existe
            SceneManager.LoadScene("Nivel 1");
        }
    }

    // Método para el botón Salir
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}