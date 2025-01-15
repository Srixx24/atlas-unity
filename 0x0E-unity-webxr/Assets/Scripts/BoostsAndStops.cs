using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostsAndStops : MonoBehaviour
{
    public GameObject saw;
    public GameObject[] boosts;
    public Transform spawnArea;
    public float spawnInterval = 2f;
    private List<GameObject> activeSaws = new List<GameObject>();
    private int maxSaws = 8;
    private GameObject ball;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // Check if the colliding object is the ball
        {
            StartCoroutine(SpawnSaws());
        }
    }

    private IEnumerator<WaitForSeconds> SpawnSaws()
    {
        while (activeSaws.Count < maxSaws)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Check for overlap
            if (!IsOverlapping(spawnPosition))
            {
                GameObject newSaw = Instantiate(saw, spawnPosition, Quaternion.identity);
                activeSaws.Add(newSaw);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Randomly generate a position within the bounds of the lane
        float spawnAreaWidth = spawnArea.localScale.x;
        float spawnAreaLength = spawnArea.localScale.z;

        float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float randomZ = Random.Range(0, spawnAreaLength); // Spawning only on the lane's length

        return new Vector3(randomX, 0.5f, randomZ);
    }

    private bool IsOverlapping(Vector3 position)
    {
        foreach (GameObject saw in activeSaws)
            if (Vector3.Distance(position, saw.transform.position) < 1f) // Check for overlap
                return true;
        return false;
    }
    
    private void Update()
    {
        for (int i = activeSaws.Count - 1; i >= 0; i--)
        {
            GameObject saw = activeSaws[i];

            if (saw == null)
            {
                activeSaws.RemoveAt(i);
                continue;
            }

            // Check all balls in the scene
            Collider[] balls = Physics.OverlapSphere(saw.transform.position, 1f, LayerMask.GetMask("Ball"));

            foreach (Collider ball in balls)
                Destroy(ball.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if triggered
        if (other.CompareTag("Ball"))
            foreach (GameObject boost in boosts)
                if (other.bounds.Intersects(boost.GetComponent<Collider>().bounds))
                        StartCoroutine(BoostSpeed(other.gameObject, 5f, 2f));
    }

    private IEnumerator BoostSpeed(GameObject ball, float amount, float duration)
    {
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.velocity += ball.transform.forward * amount; // Increase the ball's speed
            yield return new WaitForSeconds(duration); // Wait for the boost duration
            ballRigidbody.velocity -= ball.transform.forward * amount; // Revert speed back to normal
        }
    }
}