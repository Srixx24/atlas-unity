using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlaneSelection : MonoBehaviour
{
    public GameManager gameManager;
    private static ARPlane selectedPlane;
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager planeManager;
    [SerializeField] private GameObject startButton;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>(); // Store results of raycasting hits
    private List<ARPlane> activePlanes = new List<ARPlane>(); // Track active planes

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

        // Check for component assignments
        if (aRRaycastManager == null)
        {
            Debug.LogError("!!!!ARRaycastManager is not assigned or missing!!!!!");
        }
        if (planeManager == null)
        {
            Debug.LogError("!!!!ARPlaneManager is not assigned or missing!!!!!");
        }
    }

    private void Start()
    {
        if (gameManager != null && startButton != null)
        {
            Button button = startButton.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // Clear existing listeners
                button.onClick.AddListener(OnStartButtonClicked); // Add the new listener
            }
        }
        startButton.SetActive(false);
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;  // Only respond to the first finger (index 0)

        if (aRRaycastManager == null)
        {
            Debug.LogError("ARRaycastManager is not assigned or missing!");
            return;
        }

        // Raycast to detect planes at touch positions
        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            if (hits.Count > 0)
            {
                foreach (ARRaycastHit hit in hits)
                {
                    ARPlane plane = planeManager.GetPlane(hit.trackableId);
                    if (plane != null && plane.alignment == PlaneAlignment.HorizontalUp)
                    {
                        SelectPlane(plane);
                        break; // Exit after selecting the first valid plane
                    }
                }
            }
            else
            {
                Debug.LogWarning("No planes detected.");
            }
        }
        else
        {
            Debug.LogWarning("Raycast failed.");
        }
    }

    public ARPlane GetSelectedPlane()
    {
        return selectedPlane;
    }

    public void SelectPlane(ARPlane plane)
    {
        // Deactivate the previous selected plane
        if (selectedPlane != null)
        {
            selectedPlane.gameObject.SetActive(false);
        }

        // Set the new selected plane
        selectedPlane = plane;

        // Disable all other planes
        foreach (var trackedPlane in planeManager.trackables)
        {
            if (trackedPlane != selectedPlane)
            {
                trackedPlane.gameObject.SetActive(false);
            }
        }

        // Add the selected plane to the active planes list
        if (!activePlanes.Contains(plane))
        {
            activePlanes.Add(plane);
        }

        // Enable the start button
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }
    
    public void OnStartButtonClicked()
    {
        gameManager.StartGame();
    }

    public void ClearPlanes()
    {
        // Destroy all active planes
        foreach (ARPlane plane in activePlanes)
        {
            if (plane != null)
            {
                Destroy(plane.gameObject);
            }
        }
        activePlanes.Clear(); // Clear the list
    }
}