using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Collider zombieCollider;
    private PlayerController playerController;

    private int vida = 100;
    private int danoPorBala = 20;

    private bool canDamagePlayer = true;
    private float attackCooldown = 1.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        zombieCollider = GetComponent<Collider>();

        // Buscamos el GameObject "Player" y obtenemos su PlayerController
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerController = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("No se encontró el objeto 'Player' en la escena.");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            agent.destination = playerController.transform.position;
        }

        bool isMoving = agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool("isWalking", isMoving);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (vida <= 0) return;

            int damageReceived = DamageManager.Instance.doubleDamage ? danoPorBala * 2 : danoPorBala;
            vida -= damageReceived;
            Debug.Log($"¡Impacto! Vida restante del zombie: {vida}");

            if (vida <= 0)
            {
                animator.SetBool("isDeath", true);
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                zombieCollider.enabled = false; // Desactiva el collider al morir
                Destroy(gameObject, 1.5f);
            }

            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canDamagePlayer && playerController != null)
        {
            StartCoroutine(DealDamageToPlayer());
        }
    }

    IEnumerator DealDamageToPlayer()
    {
        canDamagePlayer = false;
        playerController.TakeDamage(5);
        Debug.Log("El zombie golpeó al jugador (-5 vida)");

        yield return new WaitForSeconds(attackCooldown);
        canDamagePlayer = true;
    }
}
