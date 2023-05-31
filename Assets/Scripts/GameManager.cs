using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public void StartGame()
    {
        SceneManager.LoadScene("Loading");
    }

    public void LoadSelectedScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        SpawnPointManager.instance.ResetLists();
        AudioManager.instance.PlayBackgroundAudio();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

}
