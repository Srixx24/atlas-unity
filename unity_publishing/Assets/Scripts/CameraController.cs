using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Player object
    public GameObject player;
    // Camera distance
    private Vector3 distance;


    void Start()
    {
        // Offset between camera and player
        distance = transform.position - player.transform.position;

    }

    // Called once per frame after all updates are complete
    void LateUpdate()
    {
        // Maintains offset distance
        transform.position = player.transform.position + distance;
    }
}
