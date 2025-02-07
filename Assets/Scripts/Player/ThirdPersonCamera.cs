using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // Referencia al jugador
    public Vector3 offset = new Vector3(0, 3, -6);  // Posición inicial de la cámara (más arriba y hacia atrás)
    public float sensitivity = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float minRotationY = -40f;  // Limitar la rotación vertical
    public float maxRotationY = 40f;   // Limitar la rotación vertical

    private float rotationX;
    private float rotationY;
    private float currentZoom;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentZoom = offset.magnitude;
    }

    void Update()
    {
        // Rotación de la cámara con el mouse
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minRotationY, maxRotationY); // Limitar la rotación vertical

        // Zoom con la rueda del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom - scroll, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Aplicar la rotación de la cámara
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // Establecer una posición ligeramente más alta y hacia la izquierda
        Vector3 desiredPosition = player.position + rotation * offset;

        // Aplicar la posición y mantener la vista del jugador
        transform.position = desiredPosition;
        transform.LookAt(player.position + Vector3.up * 1.5f); // Ajusta la altura del punto de enfoque
    }
}
