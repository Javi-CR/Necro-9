using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] private string levelToLoad; // Nombre del nivel a cargar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Asegurarse de que solo el jugador toque el portal
            LevelLoader.LoadLevel(levelToLoad);
        }
    }
}