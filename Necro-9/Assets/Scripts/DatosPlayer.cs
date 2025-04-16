using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatosPlayer : MonoBehaviour
{
    public int vidaPlayer;
    public Slider vidaVisual; 

    public void Update()
    {

        vidaVisual.GetComponent<Slider>().value = vidaPlayer; 


        if(vidaPlayer <= 0)
        {
            Debug.Log("GAME OVER");
        }

    }


}
