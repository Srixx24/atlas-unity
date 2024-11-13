using UnityEngine;

// Note to me <3: Bow is finally fix, its not perfect but its workable.
// This should give me a decent place to work from at least. Adjust
// Static Bow object values in inspector as needed for future tweeks.

public class StaticBow : MonoBehaviour
{
    public GameObject bow;
    public Camera arCamera;
    public float distanceFromCamera = 1.0f;
    public Vector3 positionOffset = new Vector3(0, -0.06f, 0.2f);
    public Vector3 rotationOffset = new Vector3(0, -90, 0);


    void Start()
    {
        if (bow != null)
        {
            bow.transform.localPosition = positionOffset;
            bow.transform.localRotation = Quaternion.Euler(rotationOffset);
            bow.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    void Update()
    {
        if (arCamera != null && bow != null)
        {
            // Get the camera's position and direction
            Vector3 cameraPosition = arCamera.transform.position;
            Vector3 cameraDirection = arCamera.transform.forward;

            // Calculate the new position for the bow
            Vector3 staticBowPosition = cameraPosition + cameraDirection * distanceFromCamera + positionOffset;

            // Update the bow's position
            bow.transform.position = staticBowPosition;

            // Make the bow look in the direction of the camera, may add adjustments here later
            bow.transform.rotation = Quaternion.LookRotation(cameraDirection) * Quaternion.Euler(rotationOffset);
        }
    }
}