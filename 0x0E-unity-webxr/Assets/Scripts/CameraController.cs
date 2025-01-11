using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float zoomSpeed = 4f;
    public float minZoom = -10f;
    public float maxZoom = 0f;
    public GameObject hiddenCanvas;
    private float currentZoom = 0f;
    public GameObject anchor;
    private Vector3 offset;
    private float yaw = 0f; // Horizontal rotation
    private float pitch = 0f; // Vertical rotation


    void Start()
    {
        hiddenCanvas.SetActive(false);
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    void LateUpdate()
    {
        PositionCamera(); // Update the camera's position
    }

    void PositionCamera()
    {
        Vector3 desiredPosition = anchor.transform.position + Quaternion.Euler(0, yaw, 0) * offset;
        transform.position = desiredPosition;
        transform.LookAt(anchor.transform.position);
    }

    // Rotates camera direction by holding down the right mouse button
    // Added some tilt prevention for my motion sickness lol
    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

            yaw += horizontal;
            pitch -= vertical;

            // Clamp pitch to prevent tilting
            pitch = Mathf.Clamp(pitch, -40f, 40f);

            // Apply the rotation
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    // Setup for zoom In/Out with Up/Down arrows
    private void HandleZoom()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            currentZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            currentZoom += zoomSpeed * Time.deltaTime;

        // Clamp the zoom level
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        offset.z = currentZoom;

        // Allow zoom indicator if appropriate
        hiddenCanvas.SetActive(currentZoom < maxZoom);
    }
}