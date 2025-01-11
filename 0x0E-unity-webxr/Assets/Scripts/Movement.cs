using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    private Transform cameraTransform;
    public CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Initialize movement direction
        Vector3 moveDirection = Vector3.zero;

        // Check for key inputs
        if (Input.GetKey(KeyCode.W))
            moveDirection += cameraTransform.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= cameraTransform.forward;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= cameraTransform.right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += cameraTransform.right;
        
        moveDirection.y = 0f; // Ensure on horizontal plane
        moveDirection.Normalize();
        transform.position += moveDirection * speed * Time.deltaTime; // Move in calculated direction

        // Rotate to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1f * Time.fixedDeltaTime);
        }
    }
}