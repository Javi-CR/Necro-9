using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoss : MonoBehaviour
{

    public int damage;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("PJ"))
        {
            coll.GetComponent<Personaje3D>().HP_Min -= damage;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
