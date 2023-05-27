using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
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
    private float spacing = 2.5f; // Distance between each spawned object

    private void Start()
    {
        totalPathLength = pathCreator.path.length;

        Debug.Log("totalPathLength: " + totalPathLength);
        if (PhotonNetwork.IsMasterClient)
        {
            while (spawnPoint <= totalPathLength)
            {
                SpawnObject();
            }
        }
       
    }


    private void SpawnObject()
    {
        // Select a random obstacle prefab from the available options
        GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        Vector3 position = pathCreator.path.GetPointAtDistance(spawnPoint);

        // Calculate the normalized tangent and normal vectors of the path
        Vector3 tangent = pathCreator.path.GetDirectionAtDistance(spawnPoint);
        Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

        // Calculate the rotation of the object based on the tangent
        //Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up);

        // Offset the initial object's position based on the normal vector
        position += -normal * spacing * 1.5f;

        // Spawn the rest of the objects with spacing and rotation
        for (int i = 0; i < 4; i++)
        {
            GameObject spawnedObj = PhotonNetwork.Instantiate(objectPrefab.name, new Vector3(position.x,objectPrefab.transform.position.y,position.z), Quaternion.identity);
            position += normal * spacing; // Add spacing in the direction of the normal vector
        }

        spawnPoint += spawnPoint;
    }
}
