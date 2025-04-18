using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public float detectionRadius = 15f;
    public float attackRange = 3f;
    public float specialAttackRange = 5f;
    public GameObject bossArena;

    [Header("Estadísticas")]
    public int maxHealth = 1000;
    public int currentHealth;
    public float moveSpeed = 3.5f;
    public float runSpeed = 7f;
    public int attackDamage = 20;
    public int specialAttackDamage = 40;

    [Header("Timing")]
    public float attackCooldown = 2f;
    public float specialAttackCooldown = 8f;

    // Variables privadas
    private bool playerInArena = false;
    private bool canAttack = true;
    private bool canSpecialAttack = true;
    private bool isDead = false;

    // Hash IDs para las animaciones (ahora usando Bool en lugar de Trigger)
    private int isIdleHash;
    private int isWalkingHash;
    private int isRunningHash;
    private int isAttacking1Hash;
    private int isAttacking2Hash;
    private int isDeadHash;

    private void Start()
    {
        // Inicialización
        currentHealth = maxHealth;

        // Obtener componentes si no están asignados
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        // Si no hay referencia al player, buscarla
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Hash IDs para optimizar la comunicación con el Animator (usando bool)
        isIdleHash = Animator.StringToHash("IsIdle");
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isAttacking1Hash = Animator.StringToHash("IsAttacking1");
        isAttacking2Hash = Animator.StringToHash("IsAttacking2");
        isDeadHash = Animator.StringToHash("IsDead");

        // Configurar el NavMeshAgent
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange - 0.5f;
        agent.autoBraking = false;  // Desactivar auto-braking para movimiento más fluido

        // Establecer estado inicial
        SetAnimationState(isIdleHash);

        // Prueba de movimiento inicial para verificar que todo funciona
        Invoke("ForceMove", 2f);

        Debug.Log("Boss inicializado. Player tag: " + (player != null ? player.tag : "Player no encontrado"));
    }

    // Método para forzar el movimiento (prueba)
    private void ForceMove()
    {
        if (player != null)
        {
            Debug.Log("Forzando movimiento hacia el jugador");
            agent.isStopped = false;
            agent.SetDestination(player.position);
            SetAnimationState(isWalkingHash);
        }
        else
        {
            Debug.LogError("No se puede forzar movimiento: jugador no encontrado");
        }
    }

    private void Update()
    {
        if (isDead) return;

        // Verificar si el jugador está en la arena
        CheckPlayerInArena();

        // Mostrar estado en consola
        Debug.Log("Player in arena: " + playerInArena + ", Distancia: " +
            (player != null ? Vector3.Distance(transform.position, player.position).ToString("F2") : "N/A"));

        if (playerInArena && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Lógica de comportamiento con logs
            if (distanceToPlayer <= attackRange && canAttack)
            {
                Debug.Log("BOSS: Iniciando ataque normal. Distancia: " + distanceToPlayer);
                StartCoroutine(Attack());
            }
            else if (distanceToPlayer <= specialAttackRange && canSpecialAttack)
            {
                Debug.Log("BOSS: Iniciando ataque especial. Distancia: " + distanceToPlayer);
                StartCoroutine(SpecialAttack());
            }
            else if (distanceToPlayer <= detectionRadius)
            {
                Debug.Log("BOSS: Persiguiendo al jugador. Distancia: " + distanceToPlayer);
                ChasePlayer(distanceToPlayer);
            }
            else
            {
                // Esperar - Idle
                agent.isStopped = true;
                SetAnimationState(isIdleHash);
                Debug.Log("BOSS: En idle - jugador fuera de rango de detección");
            }
        }
        else
        {
            // Esperar - Idle
            agent.isStopped = true;
            SetAnimationState(isIdleHash);
        }
    }

    // Método para establecer estado de animación (desactiva todos los demás)
    private void SetAnimationState(int activeStateHash)
    {
        // Desactivar todos los estados
        animator.SetBool(isIdleHash, false);
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isAttacking1Hash, false);
        animator.SetBool(isAttacking2Hash, false);

        // Activar solo el estado deseado
        animator.SetBool(activeStateHash, true);
    }

    private void CheckPlayerInArena()
    {
        // Forzar a true para pruebas iniciales
        // playerInArena = true;
        // return;

        // Enfoque más directo y confiable para detectar al jugador en la arena
        if (player != null && bossArena != null)
        {
            float distanceToArenaCenter = Vector3.Distance(player.position, bossArena.transform.position);
            float arenaRadius = bossArena.transform.localScale.x / 2;

            bool wasInArena = playerInArena;
            playerInArena = distanceToArenaCenter <= arenaRadius;

            if (playerInArena != wasInArena)
            {
                Debug.Log("Jugador " + (playerInArena ? "entró a" : "salió de") + " la arena. Distancia: " +
                          distanceToArenaCenter + ", Radio arena: " + arenaRadius);
            }
        }
        else
        {
            Debug.LogWarning("No se puede verificar arena: " +
                             (player == null ? "Player no asignado" : "BossArena no asignado"));
            // Para evitar bloqueo si falta alguna referencia
            playerInArena = true;
        }
    }

    private void ChasePlayer(float distance)
    {
        // Asegurarse de que el agente no esté detenido
        agent.isStopped = false;

        // Establecer el destino
        agent.SetDestination(player.position);

        Debug.Log("BOSS: Persiguiendo al jugador. Distancia: " + distance +
                  ", Velocidad: " + agent.speed +
                  ", Destino: " + agent.destination);

        // Seleccionar animación según distancia
        if (distance > detectionRadius / 2)
        {
            agent.speed = runSpeed;
            SetAnimationState(isRunningHash);
            Debug.Log("BOSS: Corriendo hacia el jugador");
        }
        else
        {
            agent.speed = moveSpeed;
            SetAnimationState(isWalkingHash);
            Debug.Log("BOSS: Caminando hacia el jugador");
        }
    }

    private IEnumerator Attack()
    {
        // Detener movimiento
        agent.isStopped = true;
        canAttack = false;

        Debug.Log("BOSS: Iniciando ataque normal");

        // Mirar al jugador
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Establecer animación de ataque
        SetAnimationState(isAttacking1Hash);

        // Esperar a que la animación llegue al punto de impacto
        yield return new WaitForSeconds(0.5f);

        // Detectar si el jugador está en rango de daño
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange + 0.5f)
        {
            // Aplicar daño al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);

            Debug.Log("BOSS: Golpeó al jugador con ataque normal por " + attackDamage + " de daño");
        }

        // Esperar a que termine la animación
        yield return new WaitForSeconds(0.5f);

        // Volver a Idle
        SetAnimationState(isIdleHash);

        // Esperar el cooldown
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        Debug.Log("BOSS: Ataque normal listo para usar nuevamente");
    }

    private IEnumerator SpecialAttack()
    {
        // Detener movimiento
        agent.isStopped = true;
        canSpecialAttack = false;

        Debug.Log("BOSS: Iniciando ataque especial");

        // Mirar al jugador
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Establecer animación de ataque especial
        SetAnimationState(isAttacking2Hash);

        // Esperar a que la animación llegue al punto de impacto
        yield return new WaitForSeconds(0.7f);

        // Detectar si el jugador está en rango de daño
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= specialAttackRange + 0.5f)
        {
            // Aplicar daño al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(specialAttackDamage);

            Debug.Log("BOSS: Golpeó al jugador con ataque especial por " + specialAttackDamage + " de daño");
        }

        // Esperar a que termine la animación
        yield return new WaitForSeconds(0.5f);

        // Volver a Idle
        SetAnimationState(isIdleHash);

        // Esperar el cooldown
        yield return new WaitForSeconds(specialAttackCooldown);
        canSpecialAttack = true;
        Debug.Log("BOSS: Ataque especial listo para usar nuevamente");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("BOSS: Recibió " + damage + " de daño. Salud restante: " + currentHealth);

        // Si el boss muere
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.isStopped = true;

        Debug.Log("BOSS: Ha muerto");

        // Desactivar componentes
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;

        // Reproducir animación de muerte
        SetAnimationState(isDeadHash);

        // Iniciar corrutina para cargar el nivel tras 5 segundos
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Esperar 5 segundos antes de continuar
        yield return new WaitForSeconds(4f);

        // Cargar el nivel de victoria
        LevelLoader.LoadLevel("Victoria");

    }


    // Método para visualizar el rango de detección y ataque en el editor
    private void OnDrawGizmosSelected()
    {
        // Dibujar radio de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dibujar radio de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Dibujar radio de ataque especial
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, specialAttackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bala"))
        {
            Bala bala = other.GetComponent<Bala>();
            int damage = bala != null ? bala.damage : 10;

            TakeDamage(damage);
            Destroy(other.gameObject);
        }
    }


}