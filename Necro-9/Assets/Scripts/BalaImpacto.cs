using UnityEngine;

public class BalaImpacto : MonoBehaviour
{
    public int da�o = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            other.GetComponent<DatosZombie>().vidaZombie -= da�o;

            // Si la vida llega a 0 o menos, destruir el zombie
            if (other.GetComponent<DatosZombie>().vidaZombie <= 0)
            {
                Destroy(other.gameObject);
            }
        }

        // Destruir la bala siempre que toque algo
        Destroy(gameObject);
    }
}
