using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Necesario para el NavMeshAgent

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;

    public float speed = 2.0f; // Velocidad de persecución
    public float stopDistance = 1.5f; // Distancia mínima antes de detenerse

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
        }

        agent.speed = speed; // Asignamos la velocidad al agente
    }

    void Update()
    {
        if (player != null && animator.GetBool("isDeath") == false)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > stopDistance)
            {
                agent.SetDestination(player.position); // Perseguir al jugador
                animator.SetBool("isWalking", true);  // Activar animación de caminar
            }
            else
            {
                agent.ResetPath(); // Detener el movimiento cuando esté cerca
                animator.SetBool("isWalking", false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            animator.SetBool("isDeath", true);
            agent.isStopped = true; // Detener el movimiento
            Destroy(gameObject, 1.5f); // Destruir el zombie después de 1.5 segundos
        }
    }
}
