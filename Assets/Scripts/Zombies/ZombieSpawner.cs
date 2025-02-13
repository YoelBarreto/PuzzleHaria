using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    private float spawnInterval = 0.5f;
    private float spawnRadius = 30f;
    public LayerMask groundLayer;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        Vector3 randomPosition = GetRandomNavMeshPosition();
        if (randomPosition != Vector3.zero)
        {
            Instantiate(zombiePrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomNavMeshPosition()
    {
        // Generar una posición aleatoria dentro del área de spawn
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Asegurarse de que esté en el plano horizontal

        // Intentar encontrar una posición válida en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position; // Retorna la posición válida en el NavMesh
        }

        // Si no se encuentra una posición válida, retornar Vector3.zero
        return Vector3.zero;
    }
}
