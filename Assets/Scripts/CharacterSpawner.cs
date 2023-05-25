using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private PathCreator pathCreator;
    private float spacing = 2.5f; // Distance between each spawned character

    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacters();
    }

    private void SpawnCharacters()
    {
        Vector3 position = pathCreator.path.GetPoint(0);

        // Calculate the normalized tangent and normal vectors of the path
        Vector3 tangent = pathCreator.path.GetDirection(0);
        Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

        // Calculate the rotation of the object based on the tangent
        //Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up);

        // Offset the initial object's position based on the normal vector
        position += -normal * spacing * 1.5f;

        // Spawn the rest of the objects with spacing and rotation
        for (int i = 0; i < 4; i++)
        {
            GameObject spawnedObj = Instantiate(characterPrefabs[i], position, Quaternion.identity, transform);
            
            if (spawnedObj.tag == "AI")
            {
                spawnedObj.transform.GetComponent<AIManager>().lanePosition = (1.5f) - i;
            }
           
            position += normal * spacing; // Add spacing in the direction of the normal vector
        }
    }

}
