using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        CreateRoom();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("Level1");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Level1");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Level1");
    }
}
