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

    private float minCameraY;

    public Vector3 startPosition;

    // Inverted camera
    public bool isInverted = false;


    void Start()
    {
        // Offset between camera and player
        distance = transform.position - player.transform.position;
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate the camera by holding down the right mouse button
        RotateCameraClick();
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

    public float GetMinCameraY()
    {
        return minCameraY;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera on the X-axis (look up/down)
        if (isInverted)
        {
            cameraRotationX += mouseY;
        }
        else
        {
            cameraRotationX -= mouseY;
        }

        cameraRotationX = Mathf.Clamp(cameraRotationX, -maxCameraRotationX, maxCameraRotationX);
        transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);

        // Rotate the camera on the Y-axis (look left/right)
        cameraRotationY += mouseX;
    }

    void RotateCameraClick()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (isInverted)
            {
                cameraRotationX += mouseY;
            }
            else
            {
                cameraRotationX -= mouseY;
            }
            cameraRotationX = Mathf.Clamp(cameraRotationX, -maxCameraRotationX, maxCameraRotationX);
            cameraRotationY += mouseX;

            transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);
        }
    }

    public void ResetCameraPosition()
    {
        // Reset the camera's position and rotation to the initial state
        transform.position = startPosition + distance;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        cameraRotationX = 0f;
        cameraRotationY = 0f;
    }
}
