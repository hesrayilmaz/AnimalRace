using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private RectTransform scoreboard;

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

    public void CenterScoreboard()
    {
        scoreboard.anchoredPosition = new Vector2(960, -540);
        scoreboard.localScale = new Vector2(2, 2);
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
