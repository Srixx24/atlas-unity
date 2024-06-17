using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Player object
    public GameObject player;
    // Camera distance
    private Vector3 distance;

    // Camera rotation sensitivity
    public float mouseSensitivity = 2f;
    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;
    private float maxCameraRotationX = 90f;


    void Start()
    {
        // Offset between camera and player
        distance = transform.position - player.transform.position;

    }

    void Update()
    {
        // Rotate the camera by holding down the right mouse button
        RotateCameraWithRightClick();
    }

    // Called once per frame after all updates are complete
    void LateUpdate()
    {
        // Maintain offset distance
        transform.position = player.transform.position + distance;

        // Rotate the camera based on mouse input
        PositionCamera();
        RotateCamera();
    }

    // Update the camera's position based on the player
    void PositionCamera()
    {
        
        transform.position = player.transform.position + Quaternion.Euler(cameraRotationX, cameraRotationY, 0f) * distance;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera on the X-axis (look up/down)
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -maxCameraRotationX, maxCameraRotationX);
        transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);

        // Rotate the camera on the Y-axis (look left/right)
        cameraRotationY += mouseX;
    }

    void RotateCameraWithRightClick()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            cameraRotationX -= mouseY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -maxCameraRotationX, maxCameraRotationX);
            cameraRotationY += mouseX;

            transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);
        }
    }
}
