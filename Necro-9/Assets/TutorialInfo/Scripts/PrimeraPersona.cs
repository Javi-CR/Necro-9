using UnityEngine;

public class PrimeraPersona : MonoBehaviour
{
    public float sensibilidad = 2f; // Sensibilidad del mouse
    public Transform jugador; // Referencia al cuerpo del jugador

    private float rotacionX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor al centro de la pantalla
    }

    void Update()
    {
        // Obtener el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        // Rotar la cámara en el eje X (mirar arriba y abajo)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limitar la rotación para evitar giros completos
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // Rotar el cuerpo del jugador en el eje Y (girar izquierda y derecha)
        jugador.Rotate(Vector3.up * mouseX);
    }
}
