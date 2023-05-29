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
    private float maxWaitTime = 1f;
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
        PhotonNetwork.JoinOrCreateRoom("Level1", new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        
        //SceneManager.LoadScene("Lobby");
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
        Debug.Log("player count: " + PhotonNetwork.PlayerList.Length);
   
        SceneManager.LoadScene("Level1");
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
}
