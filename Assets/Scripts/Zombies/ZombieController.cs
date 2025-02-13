using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    public Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {

        agent.destination = player.position;

        // Verifica si el zombie realmente se está moviendo
        bool isMoving = agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool("isWalking", isMoving);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            animator.SetBool("isDeath", true); // Activar animación de muerte
            agent.isStopped = true; // Detener movimiento
            agent.velocity = Vector3.zero; // Asegurar que se detenga por completo

            Destroy(gameObject, 1.5f); // Destruir después de 1.5 segundos
        }
    }
}
