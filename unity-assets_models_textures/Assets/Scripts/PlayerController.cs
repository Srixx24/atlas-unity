using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb; 

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Player speed
    public float speed = 0; 

    // Jump force
    public float jumpForce = 5f;
    public Vector3 jump;

    // Reference to the CameraController script
    public CameraController cameraController;

    // Fall logic
    private bool isOnPlatform = true;
    private Vector3 startPosition = new Vector3(0f, 1.25f, 0f);
    private float fallSpeed = 10f;

    private bool jumpInputReceived = false;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store player Rigidbody.
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        isOnPlatform = true;

        cameraController = FindObjectOfType<CameraController>();
    
        if (cameraController == null)
        {
            Debug.LogError("PlayerController: CameraController not found.");
        }
    }
 
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    void Update()
    {
        // Adds jump fuctionality
    	if(Input.GetKeyDown(KeyCode.Space) && isOnPlatform)
        {
    		rb.AddForce(jump * jumpForce, ForceMode.Impulse);
    		isOnPlatform = false;
    	}
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate() 
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);

        if (jumpInputReceived && isOnPlatform)
        {
            // Apply a vertical force to the Rigidbody to make the player jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnPlatform = false;
            jumpInputReceived = false;
        }

        // Check if the player is grounded
        isOnPlatform = Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.extents.y + 1f);

        // Check if the player is falling
        if (!isOnPlatform)
        {
            // Move the player downwards with the fallSpeed
            transform.Translate(Vector3.down * fallSpeed * Time.fixedDeltaTime);

            // Check if the player has reached the start position
            if (transform.position.y <= startPosition.y)
            {
                // Teleport the player to the start position
                Vector3 resetPosition = new Vector3(startPosition.x, startPosition.y + 100f, startPosition.z);
                transform.position = resetPosition;
                isOnPlatform = true;

                // Reset the player's velocity
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the entered collider is a platform
        if (collision.CompareTag("Platform"))
        {
            // Player is on platform
            isOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // Check if the exited collider is a platform
        if (collision.CompareTag("Platform"))
        {
            // Player is not on platform
            isOnPlatform = false;
        }
    }
}
