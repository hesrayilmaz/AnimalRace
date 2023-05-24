using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;
    [SerializeField] private Transform objectContainer;
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float obstacleSpawnSpeed = 2f;
    [SerializeField] private float distanceBetweenObstacles = 10f;

    private float currentDistance = 0f;
    private float totalPathLength;
    private float spawnPoint = 10f;
    
    private void Start()
    {
        totalPathLength = pathCreator.path.length;

        Debug.Log("totalPathLength: " + totalPathLength);
        while (spawnPoint <= totalPathLength)
        {
            SpawnObject();
        }
    }


    private void SpawnObject()
    {
        // Select a random obstacle prefab from the available options
        GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        Vector3 position = pathCreator.path.GetPointAtDistance(spawnPoint);
        //Quaternion rotation = pathCreator.path.GetRotationAtDistance(spawnPoint);

        // Create the obstacle GameObject at the calculated position
        GameObject obj = Instantiate(objectPrefab, position, Quaternion.identity, objectContainer);

        spawnPoint += spawnPoint;
    }
}
