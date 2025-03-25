using UnityEngine;

public class Seguimiento : MonoBehaviour
{
    public Transform jugador;  // Referencia al jugador
    public float velocidad = 3f; // Velocidad de movimiento
    public float distanciaDeteccion = 10f; // Distancia a la que detecta al jugador

    void Update()
    {
        if (jugador != null)
        {
            float distancia = Vector3.Distance(transform.position, jugador.position);

            // Si el jugador está dentro del rango de detección
            if (distancia < distanciaDeteccion)
            {
                // Mover el enemigo hacia el jugador
                Vector3 direccion = (jugador.position - transform.position).normalized;
                transform.position += direccion * velocidad * Time.deltaTime;

                // Hacer que el enemigo mire hacia el jugador
                transform.LookAt(jugador);
            }
        }
    }
}