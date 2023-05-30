using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform objectContainer;
    [SerializeField] private PathCreator pathCreator;
    private float distanceBetweenObjects = 60f;
    private float distanceBetweenObstacles = 120f;

    private Vector3[] spawnPositions;
    private float totalPathLength;
    private float spawnObjectPoint = 50f;
    private float spawnObstaclePoint = 100f;
    private float spacing = 2.5f; // Distance between each spawned object

    private void Start()
    {
        totalPathLength = pathCreator.path.length;
        spawnPositions = new Vector3[4];
        Debug.Log("totalPathLength: " + totalPathLength);

        if (PhotonNetwork.IsMasterClient)
        {
            while (spawnObjectPoint <= totalPathLength)
            {
                SpawnObject();
            }

            while (spawnObstaclePoint <= totalPathLength)
            {
                SpawnObstacle();
            }
        }
    }

    private void SpawnObject()
    {
        // Select a random obstacle prefab from the available options
        GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        Vector3 position = pathCreator.path.GetPointAtDistance(spawnObjectPoint);

        // Calculate the normalized tangent and normal vectors of the path
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(spawnObjectPoint);
        Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

        // Calculate the rotation of the object based on the normal
        Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);

        // Offset the initial object's position based on the normal vector
        position += -normal * spacing * 1.5f;

        // Spawn the rest of the objects with spacing and rotation
        for (int i = 0; i < 4; i++)
        {
            GameObject spawnedObj = PhotonNetwork.Instantiate(objectPrefab.name, new Vector3(position.x,objectPrefab.transform.position.y,position.z), rotation);
            position += normal * spacing; // Add spacing in the direction of the normal vector
        }

        spawnObjectPoint += distanceBetweenObjects;
    }

    private void SpawnObstacle()
    {
        Vector3 position = pathCreator.path.GetPointAtDistance(spawnObstaclePoint);

        // Calculate the normalized tangent and normal vectors of the path
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(spawnObstaclePoint);
        Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

        // Offset the initial object's position based on the normal vector
        position += -normal * spacing * 1.5f;

        // Spawn the rest of the objects with spacing and rotation
        for (int i = 0; i < 4; i++)
        {
            spawnPositions[i] = position;
            position += normal * spacing; // Add spacing in the direction of the normal vector
        }
        
        int randomObstacleCount = Random.Range(1, 3);
        for(int i=0; i<randomObstacleCount; i++)
        {
            int randomPosition = Random.Range(0, spawnPositions.Length);
            Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);
            GameObject spawnedObj = PhotonNetwork.Instantiate(obstaclePrefab.name, 
                new Vector3(spawnPositions[randomPosition].x, obstaclePrefab.transform.position.y, spawnPositions[randomPosition].z),
                rotation);
        }

        spawnObstaclePoint += distanceBetweenObstacles;
    }
}
