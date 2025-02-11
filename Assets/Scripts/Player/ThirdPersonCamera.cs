using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float sensitivity = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float minRotationY = -40f;
    public float maxRotationY = 40f;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minRotationY, maxRotationY);

        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 2f;
        currentZoom = Mathf.Clamp(currentZoom - scroll, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        if (player == null) return;

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        Vector3 zoomedOffset = offset.normalized * currentZoom;

        Vector3 desiredPosition = player.position + rotation * zoomedOffset;

        transform.position = desiredPosition;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}