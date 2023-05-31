using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    private bool readyToStart = false;
    private bool isGameStarted = false;
    [SerializeField] private TextMeshProUGUI countdown;
    private float timer;
    private float maxWaitTime = 5f;
    private int playerCount;
    [SerializeField] private PhotonView myPhotonView;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined to lobby");
        if (PhotonNetwork.IsMasterClient)
        {
            JoinRoom();
        }
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.All, timer);
        }
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        if (playerCount == 4)
        {
            readyToStart = true;
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timer = timeIn;
    }


    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            countdown.text = string.Format("{0:00}",timer);
            if (timer <= 0)
            {
                if (isGameStarted)
                    return;
                StartGame();
            }
        }
    }


    private void StartGame()
    {
        isGameStarted = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;

        PlayerPrefs.SetInt("PlayerCount", PhotonNetwork.PlayerList.Length);
        Debug.Log("PhotonNetwork.CurrentRoom.Name " + PhotonNetwork.CurrentRoom.Name);

        if(PhotonNetwork.CurrentRoom.Name == "Level1")
            PhotonNetwork.LoadLevel("Level1");
        else if (PhotonNetwork.CurrentRoom.Name == "Level2")
            PhotonNetwork.LoadLevel("Level2");
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("joined room");
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
        {
            timer = maxWaitTime;
        }
    }


    private void JoinRoom()
    {
        RoomOptions roomOptions1 = new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true };
        //roomOptions1.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "LevelName", "Level1" } };
        //roomOptions1.CustomRoomPropertiesForLobby = new string[] { "LevelName" };

        RoomOptions roomOptions2 = new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true };
        //roomOptions2.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "LevelName", "Level2" } };
        //roomOptions2.CustomRoomPropertiesForLobby = new string[] { "LevelName" };

        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        if (randomNumber < 0.5f)
        {
            Debug.Log(randomNumber);
            PhotonNetwork.JoinOrCreateRoom("Level1", roomOptions1, TypedLobby.Default);
        }
        else
        {
            Debug.Log(randomNumber);
            PhotonNetwork.JoinOrCreateRoom("Level2", roomOptions2, TypedLobby.Default);
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        JoinRoom(); 
    }
}
