using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    public Transform player;
    private int vida = 100;
    private int danoPorBala = 20; // Daño base de la bala

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
        bool isMoving = agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool("isWalking", isMoving);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            // Verifica si el daño x2 está activado
            int damageReceived = DamageManager.Instance.doubleDamage ? danoPorBala * 2 : danoPorBala;

            // Reduce la vida del zombie
            vida -= damageReceived;
            Debug.Log($"¡Impacto! Vida restante del zombie: {vida}");

            if (vida <= 0)
            {
                animator.SetBool("isDeath", true);
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                Destroy(gameObject, 1.5f);
            }

            // Destruye el proyectil después de golpear al zombie
            Destroy(other.gameObject);
        }
    }
}
