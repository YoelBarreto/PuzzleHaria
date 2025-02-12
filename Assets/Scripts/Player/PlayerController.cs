using UnityEngine;
using System.Collections;

public class Proyectil : MonoBehaviour
{
    void Start()
    {
        // Destruye el GameObject después de 3 segundos
        Destroy(gameObject, 3f);
    }
}

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;

    [Header("Player Movement")]
    public float moveSpeed = 6f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 1f;
    public float gravity = 7f;

    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    private float projectileSpeed = 60f;
    private float fireRate = 0.08f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isFiring = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        float cameraYRotation = cameraTransform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            animator.SetBool("isMoving", true);
            Quaternion rotation = Quaternion.Euler(0, cameraYRotation, 0);
            Vector3 moveDir = rotation * moveDirection;
            float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

            controller.Move(moveDir * speed * Time.deltaTime);
        } else 
            {
                animator.SetBool("isMoving", false);
            }

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && !isFiring)
        {
            isFiring = true;
            StartCoroutine(FireContinuously());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
        }
    }

    IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            LaunchProjectile();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void LaunchProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("No se ha asignado un prefab de proyectil.");
            return;
        }

        Vector3 spawnPosition = transform.position + transform.forward * 1.5f + Vector3.up * 0.6f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);

        MoveForward moveForward = projectile.GetComponent<MoveForward>();
        if (moveForward != null)
        {
            moveForward.speed = projectileSpeed;
        }

        Destroy(projectile, 0.9f);
    }
}
