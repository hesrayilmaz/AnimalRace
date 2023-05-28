using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    private Characters characters;
    private string playerCharacterName;
    private List<Vector3> spawnPositions;
    [SerializeField] private PathCreator pathCreator;
    private float spacing = 2.5f; // Distance between each spawned character

    // Start is called before the first frame update
    void Start()
    {
        characters = GameObject.Find("Characters").GetComponent<Characters>();
        spawnPositions = new List<Vector3>();
        //playerCharacter = FindCharacterByName(PlayerPrefs.GetString("SelectedCharacter", characters.GetCharacter(0).characterName)).characterPrefab;
        playerCharacterName = "Player"+PlayerPrefs.GetString("SelectedCharacter", characters.GetCharacter(0).characterName);
        SpawnCharacters();
    }

   /* private Character FindCharacterByName(string characterName)
    {
        foreach (Character character in characters.GetCharacters())
        {
            if (character.characterName == characterName)
            {
                return character;
            }
        }
        return null; // Character not found
    }*/

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
            spawnPositions.Add(position);
            
            
            //GameObject spawnedObj = Instantiate(characterPrefabs[i], position, Quaternion.identity, transform);
            
           /* if (spawnedObj.tag == "AI")
            {
                spawnedObj.transform.GetComponent<AIManager>().lanePosition = (1.5f) - i;
            }*/
           
            position += normal * spacing; // Add spacing in the direction of the normal vector
        }
        Debug.Log("spawnPositions.Count "+ spawnPositions.Count);
        int randomIndex = UnityEngine.Random.Range(0, spawnPositions.Count);
        PhotonNetwork.Instantiate(playerCharacterName, spawnPositions[randomIndex], Quaternion.identity);
        spawnPositions.RemoveAt(randomIndex);
    }

}
