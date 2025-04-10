using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //Enemigo Base
    public int rutina;
    public float cronometro;
    public float time_rutinas;
    public Animator ani;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    public bool atacando;
    public RangoBoss rango;
    public float speed;
    public GameObject[] hit;
    public int hit_Select;

    //Lanza llamas
    public bool lanza_llamas;
    public List<GameObject> pool = new List<GameObject>();
    public GameObject fire;
    public GameObject cabeza;
    private float cronometro2;

    //Salto de Ataque
    public float jump_distance;
    public bool direction_Skill;

    //Bola de Fuego
    public GameObject fire_ball;
    public GameObject point;
    public List<GameObject> pool2 = new List<GameObject>();


    //Vida
    public int fase = 1;
    public float HP_Min;
    public float HP_Max;
    public Image barra;
    public AudioSource musica;
    public bool muerto;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    public void Comportamiento_Boss() {

        if (Vector3.Distance(transform.position, target.transform.position) < 15)
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            point.transform.LookAt(target.transform.position);
            musica.enabled = true;

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)
            {
                switch (rutina)
                {
                    case 0:
                        //////Caminando////
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        ani.SetBool("walk", true);
                        ani.SetBool("run", false);

                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * speed * Time.deltaTime);
                        }

                        ani.SetBool("attack", false);
                        cronometro += 1 * Time.deltaTime;

                        if (cronometro > time_rutinas)
                        {
                            rutina = UnityEngine.Random.Range(0, 5);
                            cronometro = 0;
                        }
                        
                        break;

                    case 1:
                        //////Corriendo////
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        ani.SetBool("walk", false);
                        ani.SetBool("run", true);

                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * speed *2 * Time.deltaTime);
                        }
                        ani.SetBool("attack", false);
                        break;

                    case 2:
                        //////Lanzallamas//////
                        ani.SetBool("walk", false);
                        ani.SetBool("run", false);
                        ani.SetBool("attack", true);
                        ani.SetFloat("skills", 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        rango.GetComponent<CapsuleCollider>().enabled = false;
                        break;

                    case 3:

                        //////Salto de Ataque//////
                        if (fase == 2)
                        {
                            jump_distance += 1 * Time.deltaTime;
                            ani.SetBool("walk", false);
                            ani.SetBool("run", false);
                            ani.SetBool("attack", true);
                            ani.SetFloat("skills", 0);
                            hit_Select = 3;
                            rango.GetComponent<CapsuleCollider>().enabled = false;

                            if (direction_Skill)
                            {
                                if (jump_distance < 1f)
                                {
                                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                                }
                                transform.Translate(Vector3.forward * 8 * Time.deltaTime);
                            }
                        }
                        else
                        {
                            rutina = 0;
                            cronometro = 0;
                        }
                        break;

                    case 4:

                        //////Bola de Fuego//////
                        if (fase == 2)
                        {
                            ani.SetBool("walk", false);
                            ani.SetBool("run", false);
                            ani.SetBool("attack", true);
                            ani.SetFloat("skills", 0);
                            rango.GetComponent<CapsuleCollider>().enabled = false;
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        }
                        else
                        {
                            rutina = 0;
                            cronometro = 0;
                        }
                        break;



                }
            }


        }
    }

    public void Final_Ani()
    {
        rutina = 0;
        ani.SetBool("attack", false);
        atacando = false;
        rango.GetComponent<CapsuleCollider>().enabled = true;
        lanza_llamas = false;
        jump_distance = 0;
        direction_Skill = false;
    }

    public void Direction_Attack_Start()
    {
        direction_Skill = true;
    }

    public void Direction_Attack_Final()
    {
        direction_Skill = false;
    }


    /////////-Melee-/////////
    public void ColliderWeaponTrue() 
    {
        hit[hit_Select].GetComponent<SphereCollider>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        hit[hit_Select].GetComponent<SphereCollider>().enabled = false;
    }

    /////////Lanzallamas/////////
    public GameObject GetBala()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        GameObject obj = Instantiate(fire,cabeza.transform.position,cabeza.transform.rotation) as GameObject;
        pool.Add(obj);
        return obj;
    }

    public void LanzaLlamas_Skill()
    {
        cronometro2 += 1 * Time.deltaTime;
        if (cronometro2 > 0.1f)
        {
            GameObject obj = GetBala();
            obj.transform.position = cabeza.transform.position;
            obj.transform.rotation = cabeza.transform.rotation;
            cronometro2 = 0;
        }
    }

    public void Start_Fire()
    {
        lanza_llamas = true;
    }

    public void Stop_Fire()
    {
        lanza_llamas = false;
    }



    public GameObject Get_Fire_Ball()
    {
        for (int i = 0; i < pool2.Count; i++)
        {
            if (!pool2[i].activeInHierarchy)
            {
                pool2[i].SetActive(true);
                return pool2[i];
            }
        }
        GameObject obj = Instantiate(fire_ball, point.transform.position, point.transform.rotation) as GameObject;
        pool2.Add(obj);
        return obj;
    }

    public void Fire_Ball_Skill()
    {
        GameObject obj = Get_Fire_Ball();
        obj.transform.position = point.transform.position;
        obj.transform.rotation = point.transform.rotation;
    }

    public void Vivo()
    {
        if (HP_Min < 500)
        {
            fase = 2;
            time_rutinas = 1;
        }

        Comportamiento_Boss();

        if (lanza_llamas)
        {
            LanzaLlamas_Skill();
        }
    }

    // Update is called once per frame
    void Update()
    {
        barra.fillAmount = HP_Min / HP_Max;
        if (HP_Min > 0)
        {
            Vivo();
        }
        else
        {
            if (!muerto)
            {
                ani.SetTrigger("dead");
                musica.enabled = false;
                muerto = true;
            }
        }
    }
}
