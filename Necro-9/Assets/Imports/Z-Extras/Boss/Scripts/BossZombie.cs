using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossZombie : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public float attackRange = 2f;
    public float runRange = 6f;
    public float walkRange = 12f;
    public float attackCooldown = 2f;

    [Header("Movimiento")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Combate")]
    public Animator animator;
    public Transform player;
    public CapsuleCollider rangeTrigger;

    private int currentHealth;
    private NavMeshAgent agent;
    private bool isInCombat = false;
    private float rutinaTimer;
    private int rutina;
    private int fase = 1;
    private float lastAttackTime;

    // Referencia al componente InPlaceAnimation si existe
    private InPlaceAnimation inPlaceAnimation;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        rangeTrigger.enabled = true;
        lastAttackTime = -attackCooldown; // Para permitir atacar inmediatamente

        // Obtener referencia al componente InPlaceAnimation si existe
        inPlaceAnimation = GetComponent<InPlaceAnimation>();

        // Si tenemos el componente InPlaceAnimation, desactivarlo 
        // (ya que queremos que el enemigo se mueva)
        if (inPlaceAnimation != null)
        {
            inPlaceAnimation.enabled = false;
        }
    }

    void Update()
    {
        if (!isInCombat || currentHealth <= 0) return;

        // Obtener la distancia actual al jugador en cada frame
        float distance = Vector3.Distance(transform.position, player.position);

        // Verificar si ha pasado el tiempo de enfriamiento desde el último ataque
        bool canAttack = Time.time > (lastAttackTime + attackCooldown);

        // LÓGICA DE ANIMACIÓN SIMPLIFICADA:
        // Si está en rango de ataque y puede atacar, ejecutar animación de ataque
        if (distance <= attackRange && canAttack)
        {
            // Iniciar ataque
            lastAttackTime = Time.time;
            // Detener el movimiento durante el ataque
            StopMovement();
            // Establecer las animaciones de movimiento en falso
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            // Elegir una animación de ataque aleatoria
            int attackIndex = Random.Range(0, 3); // 0, 1, 2
            switch (attackIndex)
            {
                case 0:
                    animator.SetTrigger("Attack1");
                    break;
                case 1:
                    animator.SetTrigger("Attack2");
                    break;
                case 2:
                    animator.SetTrigger("JumpAttack");
                    break;
            }

            // Programar un reinicio de movimiento después del cooldown si el jugador se aleja
            StartCoroutine(CheckPlayerDistanceAfterAttack());
        }
        // Si está fuera del rango de ataque, manejar movimiento
        else if (distance > attackRange)
        {
            // Actualizar animaciones de movimiento según la distancia
            UpdateMovementAnimation(distance);
        }

        // Fase 2 si la vida es menor al 50%
        if (fase == 1 && currentHealth <= maxHealth / 2)
        {
            fase = 2;
            Debug.Log("Boss ha entrado en FASE 2");
        }
    }

    // Corrutina para verificar la distancia del jugador después de un ataque
    IEnumerator CheckPlayerDistanceAfterAttack()
    {
        // Esperar a que termine la animación de ataque (aproximadamente)
        yield return new WaitForSeconds(attackCooldown * 0.5f);

        // Verificar si el jugador se alejó
        float currentDistance = Vector3.Distance(transform.position, player.position);
        if (currentDistance > attackRange)
        {
            // Si el jugador se alejó, reanudar movimiento
            UpdateMovementAnimation(currentDistance);
        }
    }

    // Método específico para actualizar solo la animación según la distancia
    void UpdateMovementAnimation(float distance)
    {
        // Asegurarnos de que el agente no esté detenido
        agent.isStopped = false;

        // Hacer que el enemigo mire hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Actualizar la rutina ocasionalmente
        rutinaTimer += Time.deltaTime;
        if (rutinaTimer > 3f)
        {
            rutina = Random.Range(0, 2); // 0: caminar, 1: correr
            rutinaTimer = 0;
        }

        // Aplicar la animación y velocidad según la rutina y distancia
        if (distance <= walkRange)
        {
            if (rutina == 0) // Caminar
            {
                agent.speed = walkSpeed;
                agent.SetDestination(player.position);
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
            }
            else if (rutina == 1 && distance <= runRange) // Correr
            {
                agent.speed = runSpeed;
                agent.SetDestination(player.position);
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
            }
        }
    }

    void StopMovement()
    {
        // Detener al agente
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        agent.isStopped = true;
        animator.SetTrigger("Die");
        this.enabled = false;
        Destroy(gameObject, 5f);
    }

    public void StartCombat()
    {
        isInCombat = true;
        rangeTrigger.enabled = false;

        // Asegurarse de que InPlaceAnimation esté desactivado al iniciar combate
        if (inPlaceAnimation != null)
        {
            inPlaceAnimation.enabled = false;
        }
    }
}