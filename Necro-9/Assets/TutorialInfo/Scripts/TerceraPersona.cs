using UnityEngine;

public class TerceraPersona : MonoBehaviour
{
    public float sensibilidad = 2f; // Sensibilidad de la c�mara
    public bool invertirY = false; // Opci�n para invertir la vista vertical

    private float rotacionX = 0f;

    public void MoverCamara(Vector2 input)
    {
        float mouseX = input.x * sensibilidad;
        float mouseY = input.y * sensibilidad;

        if (invertirY)
            mouseY *= -1;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limita la rotaci�n vertical

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX); // Rota el cuerpo del jugador
    }
}
