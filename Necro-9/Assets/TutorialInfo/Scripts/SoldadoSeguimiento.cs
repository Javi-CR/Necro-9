using UnityEngine;

public class SoldadoSeguimiento : MonoBehaviour
{
    public Transform jugador; // Referencia al jugador
    public float velocidad = 3f; // Velocidad del soldado
    public float distanciaActivacion = 5f; // Distancia a la que comienza a seguirte

    private bool siguiendo = false; // Indica si el soldado ya comenzó a seguirte

    void Update()
    {
        if (jugador != null)
        {
            float distancia = Vector3.Distance(transform.position, jugador.position);

            // Si el jugador se acerca lo suficiente, el soldado empieza a seguirlo y no se detiene
            if (distancia < distanciaActivacion)
            {
                siguiendo = true;
            }

            if (siguiendo)
            {
                // Mueve al soldado hacia el jugador
                Vector3 direccion = (jugador.position - transform.position).normalized;
                transform.position += direccion * velocidad * Time.deltaTime;

                // Hace que el soldado mire al jugador
                transform.LookAt(jugador);
            }
        }
    }
}
