using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Photon.Pun;
using Photon.Realtime;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] AIPrefabs;
    private string playerCharacterName;
    private string AICharacterName;
    private int playerCount;
    private int AICount;
    private int characterCountInRace = 4;
    private int characterCountInShop;
 

    // Start is called before the first frame update
    void Start()
    {
        characterCountInShop = Characters.instance.GetCharacters().Length;
        playerCharacterName = "Player" + PlayerPrefs.GetString("SelectedCharacter", Characters.instance.GetCharacter(0).characterName);

        playerCount = PhotonNetwork.PlayerList.Length;
        AICount = characterCountInRace - playerCount;

        SpawnCharacters();

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnAI();
        }
    }

    private void SpawnCharacters()
    {
        PhotonNetwork.Instantiate(playerCharacterName, SpawnPointManager.instance.GetRandomPosition(), Quaternion.identity);
    }

    private void SpawnAI()
    {
        for (int i = 0; i < AICount; i++)
        {
            int randomCharacter = UnityEngine.Random.Range(0, characterCountInShop);
            AICharacterName = "AI" + Characters.instance.GetCharacter(randomCharacter).characterName;
            GameObject spawnedObj = PhotonNetwork.Instantiate(AICharacterName, SpawnPointManager.instance.GetRandomPosition(), Quaternion.identity);
            spawnedObj.GetComponent<AIManager>().lanePosition = (1.5f) - SpawnPointManager.instance.GetRandomIndex();
        }
    }
}