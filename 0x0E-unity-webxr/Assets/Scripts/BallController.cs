using UnityEngine;

public class BallController : MonoBehaviour
{
    public float userSpeed = 2f;
    private Rigidbody rb;
    private bool isInLane = false;
    public float forwardForce = 10f;
    public AnimatedThrow animatedThrow;
    public ScoreKeeper scoreKeeper;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Control ball movement only if its in lane
        if (isInLane)
        {
            float horizontalInput = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                horizontalInput = -1f;
            if (Input.GetKey(KeyCode.RightArrow))
                horizontalInput = 1f;

            float vrHorizontalInput = GetVRJoystickHorizontalInput(); // VR joystick input, may not need

            // Determine the movement direction
            float movementInput = horizontalInput != 0 ? horizontalInput : vrHorizontalInput;
            MoveBall(movementInput);
        }
    }

    private void MoveBall(float horizontalInput)
    {
        // In-lane movement calculations
        Vector3 movementDirection = new Vector3(horizontalInput, 0, 0);
        transform.position += movementDirection * userSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lane"))
        {
            isInLane = true;
            scoreKeeper.BallThrown();
            if (animatedThrow != null)
                animatedThrow.StartThrow();
            MoveForward();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lane"))
            isInLane = false;
    }
    private void MoveForward()
    {
        // Apply constant force while balls in lane
        rb.AddForce(transform.forward * forwardForce, ForceMode.VelocityChange);
    }

    // Placeholder for detecting VR input, can't remember if this is
    // builtin or not, may remove after I start working on VR.
    private float GetVRJoystickHorizontalInput()
    {
        return 0f;
    }
}