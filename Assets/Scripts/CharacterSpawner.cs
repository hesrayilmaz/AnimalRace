using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CharacterSpawner : MonoBehaviour
{
    private string playerCharacterName;
    private string AICharacterName;
    private int playerCount;
    private int AICount;
    private int characterCountInRace = 4;
    private int characterCountInShop;

    private List<int> availableIndices = new List<int>();
    private Vector3[] spawnPositionsArray = new Vector3[4];
    private int[] availableIndicesArray = new int[4];

    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private PhotonView myPhotonView;
    private float spacing = 2.5f; // Distance between each spawned character
    private int randomIndex;
    private bool isPositionsReceived = false;

    // Start is called before the first frame update
    void Start()
    {
        characterCountInShop = Characters.instance.GetCharacters().Length;
        playerCharacterName = "Player" + PlayerPrefs.GetString("SelectedCharacter", Characters.instance.GetCharacter(0).characterName);
        playerCount = PhotonNetwork.PlayerList.Length;
        AICount = characterCountInRace - playerCount;

        if (PhotonNetwork.IsMasterClient)
        {
            SetSpawnPositions();
            SpawnCharacters();
            SpawnAI();
        }
        else
        {
            myPhotonView.RPC("RPC_RequestPositions", RpcTarget.MasterClient);
            StartCoroutine(WaitForPositions());
        }
    }

    private IEnumerator WaitForPositions()
    {
        while (!isPositionsReceived)
        {
            yield return null;
        }
        SpawnCharacters();
    }


    private void SpawnCharacters()
    {
        GameObject player = PhotonNetwork.Instantiate(playerCharacterName, GetRandomPosition(), Quaternion.identity);
    }

    private void SpawnAI()
    {
        for (int i = 0; i < AICount; i++)
        {
            int randomCharacter = UnityEngine.Random.Range(0, characterCountInShop);
            AICharacterName = "AI" + Characters.instance.GetCharacter(randomCharacter).characterName;
            GameObject player = PhotonNetwork.Instantiate(AICharacterName, GetRandomPosition(), Quaternion.identity);
            player.GetComponent<AIManager>().lanePosition = (1.5f) - randomIndex;
        }
    }

    private void SetSpawnPositions()
    {
        Vector3 position = pathCreator.path.GetPoint(0);
        Vector3 tangent = pathCreator.path.GetDirection(0);
        Vector3 normal = new Vector3(-tangent.z, 0f, tangent.x).normalized;

        position += -normal * spacing * 1.5f;

        for (int i = 0; i < 4; i++)
        {
            spawnPositionsArray[i] = position;
            position += normal * spacing; // Add spacing in the direction of the normal vector
            availableIndices.Add(i);
            availableIndicesArray[i] = i;
        }
        /*foreach (int idx in availableIndices)
        {
            Debug.Log(idx);
        }*/
    }

    private Vector3 GetRandomPosition()
    {
        randomIndex = availableIndices[UnityEngine.Random.Range(0, availableIndices.Count)];
        //Debug.Log("randomIndex " + randomIndex);        
        availableIndices.Remove(randomIndex);
        availableIndicesArray[randomIndex] = int.MaxValue;
        //myPhotonView.RPC("RPC_SendPositions", RpcTarget.AllBuffered, availableIndicesArray, spawnPositionsArray);
        return spawnPositionsArray[randomIndex];
    }

    [PunRPC]
    private void RPC_RequestPositions(PhotonMessageInfo info)
    {
        myPhotonView.RPC("RPC_SendPositions", info.Sender, availableIndicesArray, spawnPositionsArray);
    }

    [PunRPC]
    private void RPC_SendPositions(int[] availableIdx, Vector3[] positions)
    {
        availableIndicesArray = availableIdx;
        spawnPositionsArray = positions;
        availableIndices = new List<int>();
        foreach(int idx in availableIndicesArray)
        {
            if (idx != int.MaxValue)
            {
                availableIndices.Add(idx);
            }
        }
        isPositionsReceived = true;
    }
}