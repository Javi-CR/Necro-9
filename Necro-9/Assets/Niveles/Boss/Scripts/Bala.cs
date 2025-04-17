using UnityEngine;

public class Bala : MonoBehaviour
{
    public int damage = 10; // Da�o que hace la bala

    // Si quieres que la bala se destruya con cualquier cosa que toque
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
