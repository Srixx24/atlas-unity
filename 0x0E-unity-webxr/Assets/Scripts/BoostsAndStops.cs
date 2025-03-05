using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostsAndStops : MonoBehaviour
{
    public GameObject saw;
    public GameObject spawnArea;
    public float spawnInterval = 2f;
    private List<GameObject> activeSaws = new List<GameObject>();
    private int maxSaws = 8;


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
        }
    }

    public void StartSaws()
    {
        StartCoroutine(SpawnSaws());
    }

    private IEnumerator SpawnSaws()
    {
        while (activeSaws.Count < maxSaws)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
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
        // Randomly generate position within lane
        float spawnAreaWidth = spawnArea.GetComponent<Collider>().bounds.size.x;
        float spawnAreaLength = spawnArea.GetComponent<Collider>().bounds.size.z;
        float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2) + spawnArea.transform.position.x;
        float randomZ = Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2) + spawnArea.transform.position.z;

        return new Vector3(randomX, spawnArea.transform.position.y + 0.01f, randomZ);
    }

    private bool IsOverlapping(Vector3 position)
    {
        foreach (GameObject saw in activeSaws)
            if (Vector3.Distance(position, saw.transform.position) < 1f) // Check for overlap
                return true;
        return false;
    }
}