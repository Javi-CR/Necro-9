using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InPlaceAnimation : MonoBehaviour
{
    private Vector3 startPosition;
    private Animator animator;

    // A�adir opci�n para activar/desactivar el script
    public bool stayInPlace = true;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        // Solo mantener la posici�n si stayInPlace es true
        if (stayInPlace)
        {
            // Mantiene la posici�n original en X y Z, pero permite movimiento en Y (para saltos)
            transform.position = new Vector3(startPosition.x, transform.position.y, startPosition.z);
        }
    }
}