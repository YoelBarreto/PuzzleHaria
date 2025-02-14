using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthbar;

    [Header("Animation")]
    public Animator animator;

    [Header("Player Movement")]
    private float moveSpeed = 7f;
    private float sprintMultiplier = 1.6f;
    private float jumpForce = 1f;
    private float gravity = 17f;

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
    private bool isDead = false; // Variable para rastrear si el jugador está muerto

    public ScreenManager screenManager; // Referencia al ScreenManager

    void Start()
    {
        animator = GetComponent<Animator>();
        if (controller == null)
            controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

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
        }
        else
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
        Vector3 spawnPosition = transform.position + transform.forward * 1.5f + Vector3.up * 0.6f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);

        MoveForward moveForward = projectile.GetComponent<MoveForward>();
        moveForward.speed = projectileSpeed;

        Destroy(projectile, 0.9f);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDeath", true);
        StartCoroutine(ShowDeathScreenAfterAnimation());
    }

    private IEnumerator ShowDeathScreenAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        screenManager.ShowDeathScreen();
        DisablePlayerControls();
    }

    public void OnDeathAnimationEnd()
    {
        screenManager.ShowDeathScreen();
        DisablePlayerControls();
    }

    private void DisablePlayerControls()
    {
        enabled = false;
    }
}
