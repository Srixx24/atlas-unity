using UnityEngine;
using System.Collections;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class LaunchLogic : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject trajectoryLinePrefab;
    public GameObject hitMessage;
    public GameObject missMessage;
    public GameObject shootAgain;
    public GameManager gameManager;
    public int totalShots = 7;
    private int currentShot = 0;
    private GameObject currentArrow;
    private LineRenderer trajectoryLine;
    private Vector3 startPosition;
    public GameObject[] reloadUIObjects;
    private bool isDragging = false;
    private bool readyToLaunch = false;
    public GameObject boundingBox;


    public void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();

        // Insure shoot again info is inactive
        shootAgain.SetActive(false);
    }

    public void ReadyToLaunch(bool ready)
    {
        readyToLaunch = ready;
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        EnhancedTouch.Touch.onFingerMove += OnFingerMove;
        EnhancedTouch.Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
        EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index == 0 && readyToLaunch)
        {
            startPosition = finger.screenPosition; // Store initial touch position
            StartDragging(startPosition);
        }
    }

    private void OnFingerMove(EnhancedTouch.Finger finger)
    {
        if (isDragging && finger.index == 0 && readyToLaunch)
            UpdateTrajectory(finger.screenPosition);
    }

    private void OnFingerUp(EnhancedTouch.Finger finger)
    {
        if (isDragging && finger.index == 0 && readyToLaunch)
            ShootArrow();
    }

    private void StartDragging(Vector2 touchPosition)
    {
        UIChecks();

        // Check for old arrow and remove
        if (currentArrow != null)
            Destroy(currentArrow);

        // Check for old line and remove
        if (trajectoryLine != null)
            Destroy(trajectoryLine.gameObject);

        // Calculate the world position from the touch position
        Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10));

        // Use the bounds of box for spawning
        Bounds bounds = boundingBox.GetComponent<Collider>().bounds;

        // Lock X to center
        float clampedX = (bounds.min.x + bounds.max.x) / 2;
        
        // Lock Y to center
        float clampedY = (bounds.min.y + bounds.max.y) / 2;

        // Lock Z to box dimensions
        float clampedZ = Mathf.Clamp(touchWorldPosition.z, bounds.min.z, bounds.max.z);

        // Set the start position
        startPosition = new Vector3(clampedX, clampedY, clampedZ);

        // Set the rotation for the arrow
        Quaternion rotation = Quaternion.Euler(90f, -12.508f, 167.705f);

        // Instantiate arrow and trajectory line
        currentArrow = Instantiate(arrowPrefab, startPosition, rotation);
        currentArrow.tag = "Arrow";
        currentArrow.GetComponent<Rigidbody>().isKinematic = true; // Make the arrow static initially
        trajectoryLine = Instantiate(trajectoryLinePrefab).GetComponent<LineRenderer>();
        trajectoryLine.positionCount = 0; // Initialize the line

        isDragging = true;
        ReloadTracker();
    }

    private void UpdateTrajectory(Vector2 touchPosition)
    {
        // Clear previous trajectory points
        trajectoryLine.positionCount = 0;

        // Calculate the launch direction
        Vector3 newPoint = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10));
        Vector3 launchDirection = newPoint - startPosition;

        // Apply force based on dragged
        float forceMagnitude = launchDirection.magnitude * 2.5f;

        // Calculate trajectory points
        for (float t = 0; t <= 1; t += 0.1f)
        {
            Vector3 point = startPosition + launchDirection.normalized * forceMagnitude * t + Physics.gravity * t * t * 0.5f;
            trajectoryLine.positionCount++;
            trajectoryLine.SetPosition(trajectoryLine.positionCount - 1, point);
        }
    }

    private void ShootArrow()
    {
        if (!isDragging || currentArrow == null) return;
        
        isDragging = false;

        // Calculate the launch direction and apply force
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.isKinematic = false; // Make the arrow dynamic for shooting
        Vector3 launchDirection = currentArrow.transform.position - trajectoryLine.GetPosition(trajectoryLine.positionCount - 1);
        
        // Apply force based on dragged
        float forceMagnitude = launchDirection.magnitude * 2.5f;
        rb.AddForce(launchDirection.normalized * forceMagnitude, ForceMode.Impulse);

        // Hide trajectory line
        Destroy(trajectoryLine.gameObject);

        // Handle scoring and UI update
        StartCoroutine(CheckHit());
    }

    private IEnumerator CheckHit()
    {
        // Wait for arrow to travel
        yield return new WaitForSeconds(1f);

        // Check if currentArrow is assigned
        if (currentArrow == null)
        {
            Debug.LogError("currentArrow is null!");
            yield break; // Exit the coroutine if null
        }

        // Check target collisionm
        Collider[] hitTargets = Physics.OverlapSphere(currentArrow.transform.position, 2f);
        Debug.Log("Hit Targets Count: " + hitTargets.Length);

        bool hit = false;
        foreach (var target in hitTargets)
        {
            if (target.CompareTag("Pawn") || target.CompareTag("Warrior") || target.CompareTag("King"))
            {
                hit = true;
                Debug.LogWarning("Contact was tracked as true!!!! LL");

                // Sends which target was hit
                string targetType = target.tag;

                // Notify the target hit score
                TargetHitScore targetHitScore = target.GetComponent<TargetHitScore>();
                if (targetHitScore != null)
                {
                    targetHitScore.NotifyHit(targetType);
                    Destroy(target.gameObject); // Destroy the target

                    // Ensure the SlimeController maintains the target count
                    SlimeController slimeController = Object.FindFirstObjectByType<SlimeController>();
                    if (slimeController != null)
                    {
                        slimeController.RemoveTarget(target.gameObject);
                    }

                    ShowHitMessage();
                }
                else
                    Debug.LogError("TargetHitScore component not found on " + target.name);

                break;
            }
        }

        if (!hit)
            ShowMissMessage();

        // Clean up the arrow
        Destroy(currentArrow);
        currentShot++;

        // Update the reload UI
        ReloadTracker();

        // Check shot count
        if (currentShot >= totalShots)
            gameManager.EndGame();
        else
            shootAgain.SetActive(true);
    }

    private void ReloadTracker()
    {
        if (reloadUIObjects != null && reloadUIObjects.Length > 0)
        {
            // Loop through all shots taken
            for (int i = 0; i < reloadUIObjects.Length; i++)
            {
                var image = reloadUIObjects[i].GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    Color color = image.color;
                    // Set the alpha for each
                    color.a = (i < currentShot) ? 160 / 255f : 1f;
                    image.color = color;
                }
            }
        }
    }

    private void ShowHitMessage()
    {
        hitMessage.SetActive(true);
        StartCoroutine(HideMessage(hitMessage));
    }

    private void ShowMissMessage()
    {
        missMessage.SetActive(true);
        StartCoroutine(HideMessage(missMessage));
    }

    private IEnumerator HideMessage(GameObject message)
    {
        yield return new WaitForSeconds(2f);
        message.SetActive(false);
    }

    public void UIChecks()
    {
        // Hide the starting instructions
        GameObject instructions = GameObject.FindGameObjectWithTag("Starting Launch Info");
        if (instructions != null)
            instructions.SetActive(false);

        // Hide the shoot again message if active
        GameObject shootAgainInfo = GameObject.FindGameObjectWithTag("Shoot Again");
        if (shootAgainInfo != null)
            shootAgainInfo.SetActive(false);
    }

    public void ResetArrowCount()
    {
        // Reset the shot count
        currentShot = 0;
    }

    public void ResetReload()
    {
        // Reset alpha for all reload images
        if (reloadUIObjects != null)
        {
            foreach (var obj in reloadUIObjects)
            {
                var image = obj.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
            }
        }
    }
}
