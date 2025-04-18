using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToMenuHandler : MonoBehaviour
{
    [SerializeField] private Button menuButton;

    private void Start()
    {
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogWarning("No se asign� el bot�n en el inspector.");
        }
    }

    private void GoToMainMenu()
    {
        LevelLoader.LoadLevel("Menu");
    }
}
