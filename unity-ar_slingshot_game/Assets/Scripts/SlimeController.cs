using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SlimeController : MonoBehaviour
{
    private PlaneSelection planeSelection;
    private GameManager gameManager;
    public GameObject[] targetPrefabs;
    public int numberOfTargets = 5;
    private bool targetsInstantiated = false;
    private List<GameObject> activeTargets = new List<GameObject>();


    private void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        planeSelection = Object.FindFirstObjectByType<PlaneSelection>();
    }

    public void PopulateTargets()
    {
        ARPlane activePlane = planeSelection.GetSelectedPlane();

        // Only instantiate targets if a plane is selected
        if (activePlane != null && !targetsInstantiated)
        {
            StartCoroutine(InstantiateTargets(activePlane));
        }
    }

    private IEnumerator InstantiateTargets(ARPlane activePlane)
    {
        targetsInstantiated = true; // Prevents further instantiation

        for (int i = 0; i < numberOfTargets; i++)
        {
            GameObject target = CreateTarget(activePlane);
            activeTargets.Add(target); // Add to active targets list
            yield return null;
        }
    }

    private GameObject CreateTarget(ARPlane activePlane)
    {
        // Choose a random target prefab
        GameObject targetPrefab = targetPrefabs[Random.Range(0, targetPrefabs.Length)];

        // Instantiate the target at a random position on the plane
        Vector3 randomPosition = GetRandomPositionOnPlane(activePlane);
        randomPosition.z = 20; // Test
        GameObject target = Instantiate(targetPrefab, randomPosition, Quaternion.identity);

        // Assign the ScoreKeeper to the target's TargetHitScore component
        TargetHitScore targetHitScore = target.GetComponent<TargetHitScore>();
        ScoreKeeper scoreKeeper = Object.FindFirstObjectByType<ScoreKeeper>();
        if (targetHitScore != null && scoreKeeper != null)
        {
            targetHitScore.scoreKeeper = scoreKeeper;
        }

        // Start moving the target
        StartCoroutine(MoveTarget(target, activePlane));

        return target;
    }

    public void RemoveTarget(GameObject target)
    {
        activeTargets.Remove(target); // Remove from active targets list
        Destroy(target); // Destroy the target

        // Check if new target is needed
        if (activeTargets.Count < numberOfTargets)
        {
            ARPlane activePlane = planeSelection.GetSelectedPlane();
            if (activePlane != null)
            {
                GameObject newTarget = CreateTarget(activePlane);
                activeTargets.Add(newTarget); // Add new target to the list
            }
        }
    }

    private Vector3 GetRandomPositionOnPlane(ARPlane activePlane)
    {
        // Get the center and size of the selected plane
        Vector3 planeCenter = activePlane.center;
        Vector2 planeSize = activePlane.size;

        // Generate a random position within the plane
        float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
        float randomZ = Random.Range(-planeSize.y / 2, planeSize.y / 2);

        // Position the target on the plane's Y position
        Vector3 randomPosition = new Vector3(planeCenter.x + randomX, planeCenter.y, planeCenter.z + randomZ);

        // Scale the target based on the distance from the camera
        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, randomPosition);
        float scale = Mathf.Clamp(1f / distanceFromCamera, 0.1f, 1f);
        randomPosition.y += scale; // For visibility

        return randomPosition;
    }

    private IEnumerator MoveTarget(GameObject target, ARPlane activePlane)
    {
        // Random movement direction and speed
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        float speed = Random.Range(0.5f, 2f);

        while (true)
        {
            target.transform.Translate(randomDirection * speed * Time.deltaTime, Space.World);

            // Clamp the target position within the active plane bounds
            ClampPositionToPlane(target.transform, activePlane);

            // Snap the target to the plane's height
            Vector3 targetPos = target.transform.position;
            targetPos.y = activePlane.center.y; // Set Y to the plane's height
            target.transform.position = targetPos;

            // Make the target face the camera
            Vector3 directionToCamera = Camera.main.transform.position - target.transform.position;

            directionToCamera.y = 0; // Keep the rotation on the Y axis
            if (directionToCamera != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
                target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Randomly change direction every 2 seconds
            if (Random.Range(0f, 1f) < 0.02f) // 2% chance to change direction each frame
            {
                randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            }

            yield return null;
        }
    }

    private void ClampPositionToPlane(Transform targetTransform, ARPlane activePlane)
    {
        Vector3 planeCenter = activePlane.center;
        Vector2 planeSize = activePlane.size;

        // Calculate the half extents
        float halfWidth = planeSize.x / 2;
        float halfHeight = planeSize.y / 2;

        // Clamping target position
        Vector3 clampedPosition = targetTransform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, planeCenter.x - halfWidth, planeCenter.x + halfWidth);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, planeCenter.z - halfHeight, planeCenter.z + halfHeight);
        
        targetTransform.position = clampedPosition;
    }
}