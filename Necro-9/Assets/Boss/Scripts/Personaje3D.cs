using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personaje3D : MonoBehaviour
{

    public float TransLayers;

    public Rigidbody rb;
    public float speed;
    public Animator ani;
    public Transform Eje;

    public bool inground;
    private RaycastHit hit;
    public float distance;
    public Vector3 v3;

    public GameObject Camara;
    public GameObject Arrow;
    public Vector3 v33;
    public GameObject arma;
    public bool candadoAct;
    private RaycastHit hit2;
    private bool atacando;
    public static Personaje3D me;

    public float HP_Min;
    public float HP_Max;
    public Image barra;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        barra.fillAmount = HP_Min / HP_Max;

    }
}
