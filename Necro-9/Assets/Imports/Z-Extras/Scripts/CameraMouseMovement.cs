using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseMovement : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    [Tooltip("Intensidad del movimiento horizontal")]
    [Range(0.01f, 1f)]
    public float intensidadHorizontal = 0.1f;

    [Tooltip("Intensidad del movimiento vertical")]
    [Range(0.01f, 1f)]
    public float intensidadVertical = 0.1f;

    [Tooltip("Suavizado del movimiento")]
    [Range(1f, 20f)]
    public float suavizado = 5f;

    [Tooltip("Limitar el movimiento a una zona específica")]
    public bool limitarMovimiento = true;

    [Tooltip("Límite máximo de movimiento desde la posición original")]
    [Range(0.1f, 5f)]
    public float limiteMovimiento = 1f;

    // Posición original de la cámara
    private Vector3 posicionOriginal;

    // Posición actual objetivo
    private Vector3 posicionObjetivo;

    // Referencia a la transformación de la cámara
    private Transform camara;

    private void Start()
    {
        camara = transform;
        posicionOriginal = camara.position;
        posicionObjetivo = posicionOriginal;
    }

    private void Update()
    {
        // Calcular la posición del cursor normalizada (0-1) en la pantalla
        Vector2 posicionCursorNormalizada = new Vector2(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height
        );

        // Convertir a rango -1 a 1 (centro de la pantalla = 0,0)
        Vector2 posicionCursorCentrada = new Vector2(
            (posicionCursorNormalizada.x - 0.5f) * 2f,
            (posicionCursorNormalizada.y - 0.5f) * 2f
        );

        // Calcular el desplazamiento de la cámara
        Vector3 desplazamiento = new Vector3(
            -posicionCursorCentrada.x * intensidadHorizontal,
            -posicionCursorCentrada.y * intensidadVertical,
            0
        );

        // Limitar el movimiento si está habilitado
        if (limitarMovimiento)
        {
            desplazamiento = Vector3.ClampMagnitude(desplazamiento, limiteMovimiento);
        }

        // Calcular la posición objetivo
        posicionObjetivo = posicionOriginal + desplazamiento;

        // Aplicar suavizado al movimiento
        camara.position = Vector3.Lerp(camara.position, posicionObjetivo, Time.deltaTime * suavizado);
    }

    // Método para restablecer la posición de la cámara cuando se sale del menú
    public void ResetPosition()
    {
        camara.position = posicionOriginal;
        posicionObjetivo = posicionOriginal;
    }
}