using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject mainMenuButton;

    public void StartGame()
    {
        SceneManager.LoadScene("Loading");
    }

    public void LoadSelectedScene(string sceneName)
    {
        if (sceneName == "MainMenu")
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }

    public void ShowExitButton()
    {
        mainMenuButton.SetActive(true);
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        AudioManager.instance.PlayBackgroundAudio();
        SceneManager.LoadScene("MainMenu");
    }

}
