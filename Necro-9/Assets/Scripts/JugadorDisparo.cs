using UnityEngine;

public class JugadorDisparo : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 25f;
    public float tiempoEntreDisparos = 0.2f;

    private bool puedeDisparar = true;
    private float proximoDisparo = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= proximoDisparo && puedeDisparar)
            {
                Disparar();
                proximoDisparo = Time.time + tiempoEntreDisparos;
            }
        }
    }

    void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        rb.velocity = puntoDisparo.forward * fuerzaDisparo;
    }



}