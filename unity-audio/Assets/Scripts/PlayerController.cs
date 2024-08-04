using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb; 

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Player speed and movement
    public float speed = 0;

    public float rotationSpeed = 180f;

    private Transform cameraTransform;

    // Jump force
    public float jumpForce = 10f;
    public Vector3 jump;

    // Gravity logic
    private float gravity = 20f;
    private float maxFallSpeed = 30f;

    private float horizontalVelocityDamping = 0.5f;

    // Fall logic
    private bool isOnPlatform = true;
    private Vector3 startPosition = new Vector3(0f, 1.25f, 0f);
    private float fallSpeed = 10f;
    private bool frozenFalling = false;

    private bool jumpInputReceived = false;

    // Reference to the CameraController script
    public CameraController cameraController;

    private Animator animator;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store player Rigidbody.
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        isOnPlatform = true;

        animator = GetComponent<Animator>();

        cameraController = FindObjectOfType<CameraController>();

        cameraTransform = Camera.main.transform;

        if (cameraController == null)
        {
            Debug.LogWarning("PlayerController: CameraController not found. Falling back to Camera.main.");
        }
    }
 

    void Update()
    {
        // Adds jump fuctionality
    	if(Input.GetKeyDown(KeyCode.Space) && isOnPlatform)
        {
    		rb.AddForce(jump * jumpForce, ForceMode.Impulse);
    		isOnPlatform = false;
        }
    

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (cameraTransform != null)
        {
            // Calculate the movement direction based on the camera's forward and right vectors
            Vector3 movementDirection = (cameraTransform.forward * vertical) + (cameraTransform.right * horizontal);
            movementDirection.y = 0f; // Ensure the player moves on the horizontal plane
            movementDirection.Normalize();

            // Move the player in the calculated direction
            transform.position += movementDirection * speed * Time.deltaTime;

            // Rotate the player to face the movement direction
            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
            
            // Trigger the appropriate animation based on the movement
            if (movementDirection.magnitude > 0.1f)
            {
                animator.SetFloat("IsMoving", 1);
                animator.SetTrigger("IdleToRunning");
            }
            else
            {
                animator.SetFloat("IsMoving", 0);
                animator.SetTrigger("RunningToIdle");
            }
        }
        else
        {
            Debug.LogError("PlayerController: cameraTransform is null. Unable to calculate movement direction.");
        }
    }

    private void FixedUpdate() 
    {
        if (cameraController != null)
        {
            // Create a 3D movement vector using the X and Y inputs.
            Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

            // Get the camera's forward direction
            Vector3 cameraForward = cameraController.transform.forward;
            cameraForward.y = 0f; // Ignore the vertical component of the camera's forward direction
            cameraForward.Normalize(); // Normalize the vector to ensure the movement speed is consistent

            // Create the movement vector based on the camera's forward direction and the player's input
            Vector3 movementDirection = (cameraForward * movement.z) + (cameraController.transform.right * movement.x);

            // Apply force to the Rigidbody to move the player.
            rb.AddForce(movementDirection * speed);

            // Apply horizontal movement using MovePosition()
            Vector3 newPosition = transform.position + (movement * speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
        else
        {
            Debug.LogError("PlayerController: cameraController is null. Unable to calculate movement direction.");
        }
        
    
        if (jumpInputReceived && isOnPlatform)
        {
            // Apply a vertical force to the Rigidbody to make the player jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnPlatform = false;
            jumpInputReceived = false;
        }

        // Apply gravity
        if (!isOnPlatform)
        {
            // Apply downward force based on gravity
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

            // Clamp the vertical velocity to the maximum fall speed
            if (rb.velocity.y < -maxFallSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed, rb.velocity.z);
            }
        }
        else
        {
            // Reset the horizontal velocity when the player lands
            rb.velocity = new Vector3(rb.velocity.x * horizontalVelocityDamping, rb.velocity.y, rb.velocity.z * horizontalVelocityDamping);
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

    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }
}
