using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDaño : MonoBehaviour
{

    public int damage;
    public GameObject Player; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.GetComponent<DatosPlayer>().vidaPlayer -= damage;
        }
    }

}
