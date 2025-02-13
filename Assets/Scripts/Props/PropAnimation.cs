using UnityEngine;

public class PropAnimation : MonoBehaviour
{
    private float rotationSpeed = 50f;
    private float floatAmplitude = 0.5f;
    private float floatSpeed = 2f;
    private Vector3 initialPosition;

    public float doubleDamageDuration = 5f; // Duración del daño x2

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamageManager.Instance.ActivateDoubleDamage();
            Destroy(gameObject);
        }
    }
}
