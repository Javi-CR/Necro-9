using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    public GameObject armaActiva;
    public Animator animaciones;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            armaActiva.SetActive(true);
            gameObject.SetActive(false);
            animaciones.SetBool("Arma", true);
        }
    }
}
