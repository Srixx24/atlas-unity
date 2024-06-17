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

    // Track ground contact
    private bool isGrounded;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store player Rigidbody.
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }
 
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    // Adds jump fuctionality
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
    		rb.AddForce(jump * jumpForce, ForceMode.Impulse);
    		isGrounded = false;
    	}
    }

    void OnCollisionStay()
    {
    	isGrounded = true;
    }
    
    

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate() 
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 

        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }
}
