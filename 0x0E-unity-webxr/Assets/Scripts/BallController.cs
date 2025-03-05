using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float userSpeed = 2f;
    private Rigidbody rb;
    private bool isInLane = false;
    public float forwardForce = 10f;
    public AnimatedThrow animatedThrow;
    public ScoreKeeper scoreKeeper;
    public BoostsAndStops boostsAndStops;

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
                
            // Determine the movement direction
            float movementInput = horizontalInput != 0 ? horizontalInput : horizontalInput;
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
            boostsAndStops.StartSaws();
            if (animatedThrow != null)
                animatedThrow.StartThrow();
            MoveForward();
        }
        else if (collision.gameObject.CompareTag("Saw"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lane"))
            isInLane = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boosts"))
            ApplyBoost(5f, 3f);
    }
    private void MoveForward()
    {
        // Apply constant force while balls in lane
        rb.AddForce(transform.forward * forwardForce, ForceMode.VelocityChange);
    }

    public void ApplyBoost(float amount, float duration)
    {
        StartCoroutine(BoostSpeed(amount, duration));
    }

    private IEnumerator BoostSpeed(float amount, float duration)
    {
        if (rb != null)
        {
            rb.velocity += transform.forward * amount;
            yield return new WaitForSeconds(duration);
            rb.velocity -= transform.forward * amount;
        }
    }
}