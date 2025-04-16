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

    [Header("Estad�sticas")]
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
        // Inicializaci�n
        currentHealth = maxHealth;

        // Obtener componentes si no est�n asignados
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        // Si no hay referencia al player, buscarla
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // Hash IDs para optimizar la comunicaci�n con el Animator (usando bool)
        isIdleHash = Animator.StringToHash("IsIdle");
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isAttacking1Hash = Animator.StringToHash("IsAttacking1");
        isAttacking2Hash = Animator.StringToHash("IsAttacking2");
        isDeadHash = Animator.StringToHash("IsDead");

        // Configurar el NavMeshAgent
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange - 0.5f;
        agent.autoBraking = false;  // Desactivar auto-braking para movimiento m�s fluido

        // Establecer estado inicial
        SetAnimationState(isIdleHash);

        // Prueba de movimiento inicial para verificar que todo funciona
        Invoke("ForceMove", 2f);

        Debug.Log("Boss inicializado. Player tag: " + (player != null ? player.tag : "Player no encontrado"));
    }

    // M�todo para forzar el movimiento (prueba)
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

        // Verificar si el jugador est� en la arena
        CheckPlayerInArena();

        // Mostrar estado en consola
        Debug.Log("Player in arena: " + playerInArena + ", Distancia: " +
            (player != null ? Vector3.Distance(transform.position, player.position).ToString("F2") : "N/A"));

        if (playerInArena && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // L�gica de comportamiento con logs
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
                Debug.Log("BOSS: En idle - jugador fuera de rango de detecci�n");
            }
        }
        else
        {
            // Esperar - Idle
            agent.isStopped = true;
            SetAnimationState(isIdleHash);
        }
    }

    // M�todo para establecer estado de animaci�n (desactiva todos los dem�s)
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

        // Enfoque m�s directo y confiable para detectar al jugador en la arena
        if (player != null && bossArena != null)
        {
            float distanceToArenaCenter = Vector3.Distance(player.position, bossArena.transform.position);
            float arenaRadius = bossArena.transform.localScale.x / 2;

            bool wasInArena = playerInArena;
            playerInArena = distanceToArenaCenter <= arenaRadius;

            if (playerInArena != wasInArena)
            {
                Debug.Log("Jugador " + (playerInArena ? "entr� a" : "sali� de") + " la arena. Distancia: " +
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
        // Asegurarse de que el agente no est� detenido
        agent.isStopped = false;

        // Establecer el destino
        agent.SetDestination(player.position);

        Debug.Log("BOSS: Persiguiendo al jugador. Distancia: " + distance +
                  ", Velocidad: " + agent.speed +
                  ", Destino: " + agent.destination);

        // Seleccionar animaci�n seg�n distancia
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

        // Establecer animaci�n de ataque
        SetAnimationState(isAttacking1Hash);

        // Esperar a que la animaci�n llegue al punto de impacto
        yield return new WaitForSeconds(0.5f);

        // Detectar si el jugador est� en rango de da�o
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange + 0.5f)
        {
            // Aplicar da�o al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);

            Debug.Log("BOSS: Golpe� al jugador con ataque normal por " + attackDamage + " de da�o");
        }

        // Esperar a que termine la animaci�n
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

        // Establecer animaci�n de ataque especial
        SetAnimationState(isAttacking2Hash);

        // Esperar a que la animaci�n llegue al punto de impacto
        yield return new WaitForSeconds(0.7f);

        // Detectar si el jugador est� en rango de da�o
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= specialAttackRange + 0.5f)
        {
            // Aplicar da�o al jugador
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(specialAttackDamage);

            Debug.Log("BOSS: Golpe� al jugador con ataque especial por " + specialAttackDamage + " de da�o");
        }

        // Esperar a que termine la animaci�n
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
        Debug.Log("BOSS: Recibi� " + damage + " de da�o. Salud restante: " + currentHealth);

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

        // Reproducir animaci�n de muerte
        SetAnimationState(isDeadHash);

        // Despu�s de la muerte
        Destroy(gameObject, 5f); // Destruir despu�s de 5 segundos
    }

    // M�todo para visualizar el rango de detecci�n y ataque en el editor
    private void OnDrawGizmosSelected()
    {
        // Dibujar radio de detecci�n
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dibujar radio de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Dibujar radio de ataque especial
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, specialAttackRange);
    }
}