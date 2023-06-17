using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI playerNickNameText;
    private string playerNickName;


    private void Start()
    {
        //PlayerPrefs.SetString("NickName", string.Empty);
        playerNickName = PlayerPrefs.GetString("NickName", string.Empty);
        PhotonNetwork.LocalPlayer.NickName = playerNickName;
        playerNickNameText.text = "Player Name: "+ playerNickName;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined to lobby");
        if(PlayerPrefs.GetString("NickName")!=string.Empty)
            startButton.interactable = true;
    }

    public void ReadInputName()
    {
        playerNickName = inputField.text;
        SetNickNames();
    }

    public void SelectRandomNickName()
    {
        playerNickName = "Player" + Random.Range(1, 100);
        SetNickNames();
    }

    private void SetNickNames()
    {
        PlayerPrefs.SetString("NickName", playerNickName);
        playerNickNameText.text = "Player Name: " + playerNickName;
        PhotonNetwork.LocalPlayer.NickName = playerNickName;
        Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName);
        startButton.interactable = true;
    }
}
