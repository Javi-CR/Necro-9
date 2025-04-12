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
    private bool isAttacking = false;
    private float rutinaTimer;
    private int rutina;
    private int fase = 1;

    // Referencia al componente InPlaceAnimation si existe
    private InPlaceAnimation inPlaceAnimation;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        rangeTrigger.enabled = true;

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

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
        else if (!isAttacking)
        {
            HandleMovement(distance);
        }

        // Fase 2 si la vida es menor al 50%
        if (fase == 1 && currentHealth <= maxHealth / 2)
        {
            fase = 2;
            Debug.Log("Boss ha entrado en FASE 2");
        }
    }

    void HandleMovement(float distance)
    {
        rutinaTimer += Time.deltaTime;

        // Asegurarnos de que el agente no esté detenido
        agent.isStopped = false;

        // Hacer que el enemigo mire hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        if (rutinaTimer > 3f)
        {
            rutina = Random.Range(0, 2); // 0: caminar, 1: correr
            rutinaTimer = 0;
        }

        switch (rutina)
        {
            case 0: // Caminar
                if (distance <= walkRange)
                {
                    agent.speed = walkSpeed;
                    agent.SetDestination(player.position);
                    animator.SetBool("Walk", true);
                    animator.SetBool("Run", false);
                }
                break;
            case 1: // Correr
                if (distance <= runRange)
                {
                    agent.speed = runSpeed;
                    agent.SetDestination(player.position);
                    animator.SetBool("Walk", false);
                    animator.SetBool("Run", true);
                }
                break;
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Detener el movimiento durante el ataque
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);

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

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
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