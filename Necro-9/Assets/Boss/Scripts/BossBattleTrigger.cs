using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<BossZombie>().StartCombat();
            gameObject.SetActive(false); // Desactiva el trigger
        }
    }
}