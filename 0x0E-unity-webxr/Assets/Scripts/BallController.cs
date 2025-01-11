using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;
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
        // Control ball movement only if it is in lane
        if (isInLane)
        {
            // For keyboard control
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

    // In-lane movement calculations
    private void MoveBall(float horizontalInput)
    {
        Vector3 movementDirection = new Vector3(horizontalInput, 0, 0);
        transform.position += movementDirection * speed * Time.deltaTime;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lane"))
        {
            isInLane = true;
            scoreKeeper.BallThrown();
            if (animatedThrow != null)
                animatedThrow.StartThrow();

            PushBallForward();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the ball has exited the lane
        if (collision.gameObject.CompareTag("Lane"))
            isInLane = false;
    }

    private void PushBallForward()
    {
        Vector3 pushForce = transform.forward * forwardForce;
        rb.AddForce(pushForce, ForceMode.Impulse);
    }

    // Placeholder for detecting VR input, can't remember if this is
    // builtin or not, may remove after I start working on VR.
    private float GetVRJoystickHorizontalInput()
    {
        return 0f;
    }
}